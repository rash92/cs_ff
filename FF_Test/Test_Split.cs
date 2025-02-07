using System.Linq;
using snns;
using System;


namespace Test_Split;

public class SplitIntegerCollection
{
	public readonly Func<int, bool> Predicate = int.IsEvenInteger;

	[Test]
	public void DefaultTest()
	{
		var col = new List<int> { 1, 2, 3, 4, 5, 6 };

		var whereEven = col.Where(x => Predicate(x)).ToList();
		var whereNotEven = col.Where(x => !Predicate(x)).ToList();

		var (splitEven, splitNotEven) = FF.Split(col, Predicate);

		Assert.That(FF.SameElements(whereEven, splitEven.ToList()));
		Assert.That(FF.SameElements(whereNotEven, splitNotEven.ToList()));
	}

	[Test]
	public void WillNotRearrangeValues()
	{
		var col = new List<int> { 1, 2, 3, 4, 5, 6 };

		var whereEvenList = col.Where(x => Predicate(x)).ToList();
		var whereNotEvenList = col.Where(x => !Predicate(x)).ToList();

		var (splitEven, splitNotEven) = FF.Split(col, Predicate);

		var splitEvenList = splitEven.ToList();
		var splitNotEvenList = splitNotEven.ToList();

		var count = whereEvenList.Count;
		
		Assert.That(count == splitEvenList.Count);
		Assert.That(count == whereEvenList.Count);
		Assert.That(count == splitNotEvenList.Count);
		Assert.That(count == whereNotEvenList.Count);

		for (var i = 0; i < count; i++)
		{
			Assert.That(splitEvenList[i] == whereEvenList[i]);
			Assert.That(splitNotEvenList[i] == whereNotEvenList[i]);
		}
	}

	[Test]
	public void EmptyList()
	{
		var col = new List<int>();

		var whereEven = col.Where(x => Predicate(x)).ToList();
		var whereNotEven = col.Where(x => !Predicate(x)).ToList();

		var (splitEven, splitNotEven) = FF.Split(col, Predicate);

		Assert.That(FF.SameElements(whereEven, splitEven.ToList()));
		Assert.That(FF.SameElements(whereNotEven, splitNotEven.ToList()));
	}

	[Test]
	public void BiasedRight()
	{
		var col = new List<int> { 1, 3, 5, 7, 9 };

		var whereEven = col.Where(x => Predicate(x)).ToList();
		var whereNotEven = col.Where(x => !Predicate(x)).ToList();

		var (splitEven, splitNotEven) = FF.Split(col, Predicate);

		Assert.That(FF.SameElements(whereEven, splitEven.ToList()));
		Assert.That(FF.SameElements(whereNotEven, splitNotEven.ToList()));
	}

	[Test]
	public void BiasedLeft()
	{
		var col = new List<int> { 0, 2, 4, 6, 8 };

		var whereEven = col.Where(x => Predicate(x)).ToList();
		var whereNotEven = col.Where(x => !Predicate(x)).ToList();

		var (splitEven, splitNotEven) = FF.Split(col, Predicate);

		Assert.That(FF.SameElements(whereEven, splitEven.ToList()));
		Assert.That(FF.SameElements(whereNotEven, splitNotEven.ToList()));
	}
}

public class SplitDoubleCollection
{
	public List<double?> WeirdDoubles()
	{
		return new List<double?>
		{
			double.NaN,
			double.NaN,
			null,
			1.1,
			2.2,
			double.NegativeInfinity,
			null,
			double.PositiveInfinity,
			double.NegativeZero,
			0.0
		};
	}

	[Test]
	public void WillCopyNanValues()
	{
		var col = WeirdDoubles();

		var (nan, number) = FF.Split(
			col,
			d => { return d.HasValue && double.IsNaN(d.Value); });

		Assert.That(nan.Count() == 2);
		Assert.That(number.Count() == col.Count - 2);
	}

	[Test]
	public void WillNotErroneouslyCopyNanValues()
	{
		var col = WeirdDoubles();
		var (nan, number) = FF.Split(col, d => d == double.NaN);

		Assert.That(nan.Count() == 0);
		Assert.That(number.Count() == col.Count);
	}

	[Test]
	public void WillHandleSelfReferentialPredicates()
	{
		var col = WeirdDoubles();
		var (nan, number) = FF.Split(col, d => d == col.First());

		Assert.That(nan.Count() == 0);
		Assert.That(number.Count() == col.Count);
	}
}