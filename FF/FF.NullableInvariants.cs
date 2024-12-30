using System.Collections;
using System.Reflection;

namespace snns;

public static partial class FF
{
	public static void AssertNullableInvariants<TInput>(in TInput input)
	{
		var info = new NullableInvariantsDetails.Info
		{
			Name = typeof(TInput).Name,
			IsNullable = false,
			PropertyInfo = null,
			FieldInfo = null,
			IsIndexProperty = false,
		};

		NullableInvariantsDetails.AssertNullableInvariants(input, info);
	}


	private static class NullableInvariantsDetails
	{
		public static void AssertNullableInvariants(in object? input, Info info)
		{
			try
			{
				if (input == null)
				{
					if (info.IsNullable)
					{
						return;
					}
					else
					{
						throw new InvariantException();
					}
				}

				var memberTypes = CreateTypeInfoFor(input);

				foreach (var type in memberTypes)
				{
					if (!type.IsIndexProperty)
					{
						var memberValue = type.GetValue(input);
						AssertNullableInvariants(memberValue, type);
					}
					else if (input is IEnumerable ie)
					{
						foreach (var e in ie)
						{
							AssertNullableInvariants(e, type);
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

		private const BindingFlags ReflectionFlags = BindingFlags.Public |
		                                             //BindingFlags.NonPublic |
		                                             BindingFlags.Instance;

		public static List<Info> CreateTypeInfoFor(in object obj)
		{
			var l = new List<Info>();
			var t = obj.GetType();

			l.AddRange(t.GetFields(ReflectionFlags).Select(InfoField));
			l.AddRange(t.GetProperties(ReflectionFlags).Select(InfoProperty));

			return l;
		}

		public readonly struct Info
		{
			public required string Name { get; init; }
			public required PropertyInfo? PropertyInfo { get; init; }
			public required FieldInfo? FieldInfo { get; init; }
			public required bool IsNullable { get; init; }

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
				IsNullable = ReadStateIsNullable(fieldInfo),
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
				IsNullable = ReadStateIsNullable(propertyInfo),
				IsIndexProperty = MemberIsIndexer(propertyInfo),
			};
		}

		#endregion

		#region helpers

		private static readonly NullabilityInfoContext NullabilityInfo = new();

		private static bool ReadStateIsNullable(FieldInfo fieldInfo)
		{
			var ni = NullabilityInfo.Create(fieldInfo);
			return ni.ReadState == NullabilityState.Nullable;
		}

		private static bool ReadStateIsNullable(PropertyInfo propertyInfo)
		{
			var ni = NullabilityInfo.Create(propertyInfo);
			;
			return ni.ReadState == NullabilityState.Nullable;
		}

		private static bool MemberIsIndexer(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetIndexParameters().Length != 0;
		}

		#endregion
	}
}