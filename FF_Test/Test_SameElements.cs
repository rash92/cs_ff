using snns;

namespace Test_SameElements;

public class SameElements
{
	[Test]
	public void returns_true_when_both_collections_are_empty()
	{
		Assert.That(FF.SameElements(new List<int>(),new List<int>()), Is.True);
	}
	
	[Test]
	public void returns_true_when_both_collections_have_the_same_single_element()
	{
		
		Assert.That(FF.SameElements(new List<int>{1},new List<int>{1}), Is.True);
	}
	
	[Test]
	public void returns_true_when_both_collections_have_the_same_long_count_of_elements()
	{
		Assert.That(FF.SameElements(new List<int>{3,1,4,1,5,9},new List<int>{3,1,4,1,5,9}), Is.True);
	}

	[Test]
	public void returns_false_when_collections_have_different_count()
	{

		var left  = new List<int> { 1, 1 };
		var right = new List<int> { 1, 1, 1 };
		Assert.That(FF.SameElements(left, right), Is.False);
	}

	[Test]
	public void returns_false_when_collections_have_disjoint_elements()
	{
		var left  = new List<int> { 1, 3, 5, 7, 9 };
		var right = new List<int> { 0, 2, 4, 6, 8 };
		Assert.That(FF.SameElements(left, right), Is.False);
	}

	[Test]
	public void returns_false_when_collections_are_subtly_different()
	{
		var right = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
		var left  = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		// front to back -----------------------------------^
		Assert.That(FF.SameElements(left, right), Is.False);

		right = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
		left  = new List<int> { 9, 1, 2, 3, 4, 5, 6, 7, 8 };
		// back to front--------^
		Assert.That(FF.SameElements(left, right), Is.False);

		right = new List<int> { 1, 2, 3, 4, 9, 5, 6, 7, 8 };
		left  = new List<int> { 1, 2, 3, 4, 0, 5, 6, 7, 8 };
		// in the middle--------------------^
		Assert.That(FF.SameElements(left, right), Is.False);
		
		right = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
		left  = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		// offset
		Assert.That(FF.SameElements(left, right), Is.False);
	}
}
