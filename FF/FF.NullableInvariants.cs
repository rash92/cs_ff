using System.Collections;
using System.Reflection;
using System.Security.Cryptography;

namespace snns;

public static partial class FF
{
	public static void AssertNullableInvariants<TInput>(in TInput input, uint recursionLimit = 1000)
	{
		var info = new NullableInvariantsDetails.Info
		{
			Name = typeof(TInput).Name,
			Nullability = NullableInvariantsDetails.Nullability.NotAllowed,
			PropertyInfo = null,
			FieldInfo = null,
			IsIndexProperty = false,
		};

		NullableInvariantsDetails.AssertNullableInvariants(input, info, recursionLimit);
	}


	private static class NullableInvariantsDetails
	{
		public static void AssertNullableInvariants(in object? input, Info info, uint recursionLimit)
		{
			try
			{
				switch (input, info.Nullability, recursionLimit)
				{
					case (null, Nullability.Allowed, _): return;
					case (null, _, _): throw new InvariantException(InvariantException.Reason.IllegalNullable);
					case (_, _, 0): throw new InvariantException(InvariantException.Reason.RecursionLimit);
				}

				var memberTypeInfos = CreateTypeInfoFor(input);

				foreach (var typeInfo in memberTypeInfos)
				{
					if (!typeInfo.IsIndexProperty)
					{
						var typeValue = typeInfo.GetValue(input);
						AssertNullableInvariants(typeValue, typeInfo, recursionLimit - 1);
					}
					else if (input is IEnumerable ie)
					{
						foreach (var e in ie)
						{
							AssertNullableInvariants(e, typeInfo, recursionLimit - 1);
						}
					}
				}
			}
			catch (InvariantException e)
			{
				e.PushNameOfCurrentContext(info.Name);
				throw;
			}
		}


		#region info

		private const BindingFlags ReflectionFlags =
			BindingFlags.Public |
			BindingFlags.Instance;

		private static List<Info> CreateTypeInfoFor(in object obj)
		{
			var l = new List<Info>();
			var t = obj.GetType();

			l.AddRange(t.GetFields(ReflectionFlags).Select(InfoField));
			l.AddRange(t.GetProperties(ReflectionFlags).Select(InfoProperty));

			return l;
		}

		public enum Nullability
		{
			Allowed,
			NotAllowed,
		}

		public readonly struct Info
		{
			public required string Name { get; init; }
			public required PropertyInfo? PropertyInfo { get; init; }
			public required FieldInfo? FieldInfo { get; init; }
			public required Nullability Nullability { get; init; }

			public required bool IsIndexProperty { get; init; }
			public bool IsField => FieldInfo != null;

			public object? GetValue(object o)
			{
				if (IsField)
				{
					return FieldInfo?.GetValue(o);
				}
				else
				{
					return PropertyInfo?.GetValue(o);
				}
			}
		}

		public static Info InfoField(FieldInfo fieldInfo)
		{
			return new Info
			{
				Name = fieldInfo.Name,
				PropertyInfo = null,
				FieldInfo = fieldInfo,
				Nullability = ReadStateIsNullable(fieldInfo),
				IsIndexProperty = false,
			};
		}

		public static Info InfoProperty(PropertyInfo propertyInfo)
		{
			return new Info
			{
				Name = propertyInfo.Name,
				PropertyInfo = propertyInfo,
				FieldInfo = null,
				Nullability = ReadStateIsNullable(propertyInfo),
				IsIndexProperty = MemberIsIndexer(propertyInfo),
			};
		}

		#endregion

		#region helpers

		private static readonly NullabilityInfoContext NullabilityInfo = new();

		private static Nullability ReadStateIsNullable(FieldInfo fieldInfo)
		{
			return NullabilityInfo.Create(fieldInfo).ReadState switch
			{
				NullabilityState.Nullable => Nullability.Allowed,
				_ => Nullability.NotAllowed,
			};
		}

		private static Nullability ReadStateIsNullable(PropertyInfo propertyInfo)
		{
			return NullabilityInfo.Create(propertyInfo).ReadState switch
			{
				NullabilityState.Nullable => Nullability.Allowed,
				_ => Nullability.NotAllowed,
			};
		}

		private static bool MemberIsIndexer(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetIndexParameters().Length != 0;
		}

		#endregion
	}
}