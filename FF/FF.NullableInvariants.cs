using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace snns;

public static partial class FF
{
	public static void AssertNullableInvariants<T>(in T t)
	{
		var obj = t;
		var info = new NullableInvariantsDetails.Info(typeof(T).Name);

		NullableInvariantsDetails.AssertNullableInvariants(obj, info);
	}


	private static class NullableInvariantsDetails
	{
		public static void AssertNullableInvariants(in object? obj, Info info)
		{
			if (obj == null)
			{
				if (info.IsNullable)
				{
					return;
				}
				else
				{
					throw new InvariantException(info.Name);
				}
			}

			try
			{
				var type = obj.GetType();

				var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

				foreach (var property in props)
				{
					if (property.GetIndexParameters().Length > 0) continue;
					
					var o = property.GetValue(obj);
					var i = new Info(property);
					AssertNullableInvariants(o, i);
				}

				var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

				foreach (var field in fields)
				{
					var o = field.GetValue(obj);
					var i = new Info(field);
					AssertNullableInvariants(o, i);
				}
			}
			catch (InvariantException e)
			{
				e.AddNameOfCurrentContext(info.Name);
				throw;
			}
		}

		private static readonly NullabilityInfoContext NullabilityInfoContext = new();

		private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
		                                          System.Reflection.BindingFlags.NonPublic |
		                                          System.Reflection.BindingFlags.Instance;

		// we need info about each object,
		// and we need info about each object member, if any 
		public struct Info
		{
			public static IEnumerable<Info> GetForMembersOf(object obj)
			{
				var type = obj.GetType();

				var propertyInfos = type
					.GetProperties(BindingFlags)
					.Select(propertyInfo => new Info(propertyInfo));

				var fieldInfos = type
					.GetFields(BindingFlags)
					.Select(fieldInfo => new Info(fieldInfo));

				var memberInfos = propertyInfos.Concat(fieldInfos);

				return memberInfos;
			}

			public string Name { get; private set; }
			public bool IsNullable => _flags.HasFlag(Flags.Nullable);
			public bool IsIndexType => _flags.HasFlag(Flags.IndexType);

			public Info(string name)
			{
				Name = name;
				_flags = Flags.None;
			}

			public Info(PropertyInfo info)
			{
				Name = info.Name;
				_flags = Flags.None;
				if (NullabilityInfoContext.Create(info).WriteState == NullabilityState.Nullable)
					_flags = Flags.Nullable;
				var parameters = info.GetIndexParameters();
				foreach (var p in parameters)
				{
					var attr = p.Attributes;
					//		attr.
				}
			}

			public Info(FieldInfo info)
			{
				Name = info.Name;
				_flags = Flags.None;
				if (NullabilityInfoContext.Create(info).WriteState == NullabilityState.Nullable)
					_flags = Flags.Nullable;

				//	info.GetValueDirect()
			}

			[Flags]
			private enum Flags
			{
				None = 0,
				Nullable = 1,
				IndexType = 2,
			}

			private readonly Flags _flags = Flags.None;
		}
	}
}