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
		}

		[Test]
		public void
			StringsAreNotUsefulWhenEmpty()
		{
			var str = "";
			Assert.That(FF.IsUseful(str), Is.False);
		}

		[Test]
		public void StringsAreNotUsefulWhenTheyHaveOnlyWhitespace()
		{
			var whitespace = "    \t\n\t        \n           ";
			Assert.That(FF.IsUseful(whitespace), Is.False);
		}

		[Test]
		public void NullableValueTypesAreNotUsefulWhenNull()
		{
			var nullableInt = new int?();
			Assert.That(FF.IsUseful(nullableInt), Is.False);
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
		}

		[Test]
		public void ContainersOfContainersAreNotUsefulWhenNoneOfTheSubcontainersAreUseful()
		{
			var listOfEmptyLists = new List<List<int>>
			{
				new(), new(), new()
			};
			Assert.That(FF.IsUseful(listOfEmptyLists), Is.False);
		}

		[Test]
		public void IsUseful()
		{
			Assert.That(FF.IsUseful(null), Is.False);
			Assert.That(FF.IsUseful(null), Is.False);
			Assert.That(FF.IsUseful(null), Is.False);
			Assert.That(FF.IsUseful(null), Is.False);
			Assert.That(FF.IsUseful(1));
		}
	}
}