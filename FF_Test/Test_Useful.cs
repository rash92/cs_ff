using System;
using snns;

namespace Test_Useful;

public class TestIsUseful
{
	public class NotIsUseful
	{
		[Test]
		public void
			NullIsNotUseful()
		{
			object o = null!;
			Assert.That(FF.IsUseful(o), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(o));
		}

		[Test]
		public void
			StringsAreNotUsefulWhenEmpty()
		{
			var str = "";
			Assert.That(FF.IsUseful(str), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(str));
		}

		[Test]
		public void StringsAreNotUsefulWhenTheyHaveOnlyWhitespace()
		{
			var whitespace = "    \t\n\t        \n           ";
			Assert.That(FF.IsUseful(whitespace), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(whitespace));
		}

		[Test]
		public void NullableValueTypesAreNotUsefulWhenNull()
		{
			var nullableInt = new int?();
			Assert.That(FF.IsUseful(nullableInt), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(nullableInt));
		}

		[Test]
		public void ReferenceTypesAreNotUsefulWhenNull()
		{
			System.Text.Json.JsonSerializerOptions nullReference = null!;
			Assert.That(FF.IsUseful(nullReference), Is.False);
		}

		[Test]
		public void ContainersAreNotUsefulWhenEmpty()
		{
			var emptylist = new List<int>();
			Assert.That(FF.IsUseful(emptylist), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(emptylist));
		}

		[Test]
		public void ContainersOfContainersAreNotUsefulWhenNoneOfTheSubcontainersAreUseful()
		{
			var listOfEmptyLists = new List<List<int>>
			{
				new(), new(), new()
			};
			Assert.That(FF.IsUseful(listOfEmptyLists), Is.False);
			Assert.Throws<InvariantException>(() => FF.RequireUseful(listOfEmptyLists));
		}

		[Test]
		public void AssertionMessageMatchesUselessObjectType()
		{
			var str = "";
			var message = "";
			var objectName = str.GetType().Name;
			try
			{
				FF.RequireUseful(str);
			}
			catch (Exception e)
			{
				message = e.Message;
			}

			Assert.That(message, Is.EqualTo($"Asserted that {objectName} is useful but it is not"));
		}

		[Test]
		public void IsUseful()
		{
			var i = 1;
			Assert.That(FF.IsUseful(i));
			Assert.DoesNotThrow(()=>FF.RequireUseful(i));

			var str = "hello world";
			Assert.That(FF.IsUseful(str));
			Assert.DoesNotThrow(()=>FF.RequireUseful(str));

			var list = new List<int> { 1 };
			Assert.That(FF.IsUseful(list));
			Assert.DoesNotThrow(()=>FF.RequireUseful(list));

			var listList = new List<List<string?>>
			{
				new()
				{
					null,
					null
				},
				new()
				{
					"hello",
					null
				}
			};
			Assert.That(FF.IsUseful(listList));
			Assert.DoesNotThrow(()=>FF.RequireUseful(listList));

			Nullable<int> nullableI = 1;
			Assert.That(FF.IsUseful(nullableI));
			Assert.DoesNotThrow(()=>FF.RequireUseful(nullableI));
		}
	}
}