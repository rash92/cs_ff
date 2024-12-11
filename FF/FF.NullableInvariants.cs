using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace snns;

public static partial class FF
{
	public static void AssertNullableInvariants<T>(T? t)
	{ 
		
		NullableInvariantsDetails.Recurse(
			t,
			new NullableInvariantsDetails.Info(typeof(T).Name)
		);
	}

	private static class NullableInvariantsDetails
	{
		public static void Recurse(in object? obj, Info info)
		{
			//I have t of type T, and I have rules for T, and I want to know if t obeys T
			//E.g. "T is not nullable. Is t null? If so, throw, else continue to next rule.
			//Then, I want all subjects t1,t2,t3,etc./, I want to get the rules T1, T2, T3, and then I want
			//to "foreach t+T in t1+T1..t3+T3, 
			
			
			if (ThisObjectIsLeafOfTree(obj, info)) return;

			try
			{
				var infos = Info.GetForMembersOf(obj);


				// This still doesn't work - indexed values (e.g. arrays) will throw here
				foreach (var prop in infos)
				{
					// The exception happens when, for example: param obj is a string and prop is an
					// array "Char Chars[Int32]"
					// > Hello string, please assign to my variable "o" your property 'Chars'
					// at which point the string indicates that there is no such property
					// (because arrays are indexed?) by throwing some kind of reflection exception.
					var o = prop.GetValue(obj);
					var i = new Info("prop");
					Recurse(o, i);
				}

				// var fieldInfos = obj.GetType().GetFields(BindingFlags);
				//
				// foreach (var field in fieldInfos)
				// {
				// 	Recurse(field.GetValue(obj), new Info(field));
				// }
			}
			catch (InvariantException e)
			{
				e.AddNameOfCurrentContext(info.Name);
				throw;
			}
		}

		private static bool ThisObjectIsLeafOfTree([NotNullWhen(false)] object? obj, Info info)
		{
			return (obj, info.IsNullable) switch
			{
				(null, true) => true,
				(null, false) => throw new InvariantException(info.Name),
				_ => false
			};
		}

		private static readonly NullabilityInfoContext NullabilityInfoContext = new();

		private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
		                                          System.Reflection.BindingFlags.NonPublic |
		                                          System.Reflection.BindingFlags.Instance;

		// we need info about each object
		// and we need info about each object member, if any 
		public struct Info
		{
			public static IEnumerable<Info> GetForMembersOf(object obj)
			{
				var infos = Enumerable.Empty<Info>();

				infos = infos.Concat(obj.GetType().GetProperties(BindingFlags)
					                     .Select(pi => new Info(pi)));
				infos = infos.Concat(obj.GetType().GetFields(BindingFlags).Select(fi => new Info(fi)));

				return infos;
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

			public object? GetValue(in object oBj)
			{
				throw new NotImplementedException();
			}
		}
	}
}