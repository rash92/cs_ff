using snns;

namespace Test_ContainsAnyOf;

public class ReturnsFalseWhen
{
	[Test]
	public void InputIsTwoNonOverlappingLists()
	{
		//Arrange
		var x = new List<int> { 1, 2, 3 };
		var y = new List<int> { 4, 5, 6 };

		//Act
		var result = FF.ContainsAnyOf(x, y);

		//Assert
		Assert.That(result, Is.EqualTo(false));
	}

	[Test]
	public void OuterIsNullOrEmpty()
	{
		List<int>? outer = null;
		List<int>? inner = new List<int> { 1, 2, 3 };

		Assert.That(FF.ContainsAnyOf(outer, inner), Is.EqualTo(false));
		outer = new();
		Assert.That(FF.ContainsAnyOf(outer, inner), Is.EqualTo(false));
	}

	[Test]
	public void InnerIsNullOrEmpty()
	{
		List<int>? outer = new List<int> { 1, 2, 3 };
		List<int>? inner = null;

		Assert.That(FF.ContainsAnyOf(outer, inner), Is.EqualTo(false));
		inner = new();
		Assert.That(FF.ContainsAnyOf(outer, inner), Is.EqualTo(false));
	}
}

public class ReturnsTrueWhen
{
	[Test]
	public void OneSharedElement()
	{
		var x = new List<int> { 1, 2, 3 };
		var y = new List<int> { 1 };
		Assert.That(FF.ContainsAnyOf(x, y), Is.EqualTo(true));
		Assert.That(FF.ContainsAnyOf(y, x), Is.EqualTo(true));
	}

	[Test]
	public void MultipleSharedElements()
	{
		var x = new List<int> { 1, 2, 3 };
		var y = new List<int> { 1, 2 };
		Assert.That(FF.ContainsAnyOf(x, y), Is.EqualTo(true));
		Assert.That(FF.ContainsAnyOf(y, x), Is.EqualTo(true));
	}

	[Test]
	public void AllSharedElements()
	{
		var x = new List<int> { 1, 2, 3 };
		var y = new List<int> { 1, 2, 3 };
		Assert.That(FF.ContainsAnyOf(x, y), Is.EqualTo(true));
		Assert.That(FF.ContainsAnyOf(y, x), Is.EqualTo(true));
	}

	[Test]
	public void SharedElementsOutOfOrder()
	{
		var x = new List<int> { 1, 2, 3 };
		var y = new List<int> { 3, 2, 1 };
		Assert.That(FF.ContainsAnyOf(x, y), Is.EqualTo(true));
		Assert.That(FF.ContainsAnyOf(y, x), Is.EqualTo(true));
	}
}

public class DoesNotThrowForIntCollectionsWhen
{
	[Test]
	public void InnerIsNull()
	{
		var outer = new List<int> { 1, -2, 3, -4, 5, 5, 0 };
		List<int>? inner = null;
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterIsNull()
	{
		List<int>? outer = null;
		var inner = new List<int> { 1, 2, 2, 0, 3, -4 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterAndInnerAreNull()
	{
		List<int>? outer = null;
		List<int>? inner = null;
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerContainsNull()
	{
		var outer = new List<int?> { 1, 2, 0, 2, 3, 4, -5, 6 };
		var inner = new List<int?> { null, 2, 3, 4, 0, -5, 5, -6 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterContainsNull()
	{
		var outer = new List<int?> { 1, null, 2, 3, -4, 5, 6, 0 };
		var inner = new List<int?> { 1, 2, 3, -4, 5, 0, 5, 6 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerAndOuterBothContainNull()
	{
		var outer = new List<int?> { 1, null, 2, 3, 4, 5, 6, -3, -3, 0 };
		var inner = new List<int?> { 1, 2, 3, null, 5, 6 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerIsEmpty()
	{
		var outer = new List<int?> { 1, 2, 3, 4, 5, 6, -3, -3, 0 };
		var inner = new List<int?> { };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterIsEmpty()
	{
		var outer = new List<int?> { };
		var inner = new List<int?> { 1, 2, 3, 4, 5, 6, -3, -3, 0 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerAndOuterBothAreEmpty()
	{
		var outer = new List<int?> { };
		var inner = new List<int?> { };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void NoElementsInCommon()
	{
		var outer = new List<int?> { 1, 3, 5, 7, 9 };
		var inner = new List<int?> { 2, 4, 6, 8 };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}
}

public class DoesNotThrowWhenForStrings
{
	[Test]
	public void InnerIsNull()
	{
		var outer = new List<string> { "1", "a", "3", "4", "", "", "ba", "{" };
		List<string>? inner = null;
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterIsNull()
	{
		List<string>? outer = null;
		var inner = new List<string> { "1", "a", "3", "4", "", "", "ba", "{" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterAndInnerAreNull()
	{
		List<string>? outer = null;
		List<string>? inner = null;
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerContainsNull()
	{
		var outer = new List<string?> { "1", "a", "3", "4", "", "", "{" };
		var inner = new List<string?> { "1", "a", null, "4", "", "", "ba", "{" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterContainsNull()
	{
		var outer = new List<string?> { "1", "a", null, "4", "", "", "{" };
		var inner = new List<string?> { "1", "a", "4", "", "", "ba", "{" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerAndOuterBothContainNull()
	{
		var outer = new List<string?> { "1", "a", null, "4", "", "", "{" };
		var inner = new List<string?> { "1", "a", "4", "", "", null, "{" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerIsEmpty()
	{
		var outer = new List<string?> { "1", "a", null, "4", "", "", "{" };
		var inner = new List<string?> { };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void OuterIsEmpty()
	{
		var outer = new List<string?> { };
		var inner = new List<string?> { "1", "a", null, "4", "", "", "{" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void InnerAndOuterBothAreEmpty()
	{
		var outer = new List<string?> { };
		var inner = new List<string?> { };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void NoElementsInCommon()
	{
		var outer = new List<string?> { "", "a", "fsadsa", "12" };
		var inner = new List<string?> { "b", "fdafdav", "-12", "1" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}

	[Test]
	public void TheContainersAreOfDifferentNullability()
	{
		var outer = new List<string> { "a", "b", "c" };
		var inner = new string?[] { null, "d", "e" };
		Assert.DoesNotThrow(() => FF.ContainsAnyOf(outer, inner));
	}
}

public class IsRobustAgainst
{
	[Test]
	public void NullElements()
	{
		var x = new List<string?> { null, "", "abc" };
		var y = new List<string?> { null, null };

		Assert.That(FF.ContainsAnyOf(x, y), Is.EqualTo(true));
		Assert.That(FF.ContainsAnyOf(y, x), Is.EqualTo(true));
	}
}