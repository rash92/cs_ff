using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;

namespace snns;

public static partial class FF
{
	public static void AssertNullableInvariants<TInput>(
		in TInput input,
		int recursionLimit = 1000,
		long enumerationLimit = long.MaxValue)
	{
		var info = new NullableInvariantsImpl.Info
		{
			Name = typeof(TInput).Name,
			Nullability = NullableInvariantsImpl.Nullability.NotAllowed,
			PropertyInfo = null,
			FieldInfo = null,
			IsIndexProperty = false,
		};

		var impl = new NullableInvariantsImpl();
		impl.AssertNullableInvariants(input, info, recursionLimit, enumerationLimit);
	}


	private readonly struct NullableInvariantsImpl()
	{
		private readonly List<object> _visited = [];

		public void AssertNullableInvariants(
			object? input,
			Info info,
			int recursionLimit,
			long enumerationLimit)
		{
			try
			{
				switch (input, info.Nullability, recursionLimit < 0, enumerationLimit < 0)
				{
					case (null, Nullability.Allowed, _, _): return;
					case (null, _, _, _): throw new InvariantException(InvariantException.Reason.IllegalNullable);
					case (_, _, true, _): throw new InvariantException(InvariantException.Reason.RecursionLimit);
					case (_, _, _, true): throw new InvariantException(InvariantException.Reason.EnumerationLimit);
				}

				if (_visited.Any(o => ReferenceEquals(o, input)))
					return;
				else
					_visited.Add(input);

				var memberTypeInfos = CreateTypeInfoFor(input);

				foreach (var typeInfo in memberTypeInfos)
				{
					if (!typeInfo.IsIndexProperty)
					{
						var typeValue = typeInfo.GetValue(input);
						AssertNullableInvariants(typeValue, typeInfo, recursionLimit - 1, enumerationLimit);
					}
					else if (input is IEnumerable ie)
					{
						foreach (var e in ie)
						{
							enumerationLimit--;
							AssertNullableInvariants(e, typeInfo, recursionLimit, enumerationLimit);
						}
					}
				}
			}
			catch (InvariantException e)
			{
				e.PushNameOfCurrentContext(info.Name);
				throw;
			}

			return;
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