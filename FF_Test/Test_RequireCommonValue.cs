using snns;

namespace Test_RequireCommonValue;

public class RequireCommonValue
{
	[Test]
	public void Compiles()
	{
		var two = FF.RequireCommonValue(1, 1);
		var three = FF.RequireCommonValue(1, 1, 1);
		var enumerable = FF.RequireCommonValue(new List<int> { 1, 1, 1 });
		var commonValue = FF.RequireCommonValue(two, three, enumerable);
		Assert.That(commonValue, Is.EqualTo(1));
	}

	[Test]
	public void ReturnsTheCommonValue()
	{
		Assert.That(FF.RequireCommonValue(3, 3), Is.EqualTo(3));
		Assert.That(FF.RequireCommonValue(2, 2, 2), Is.EqualTo(2));
		Assert.That(FF.RequireCommonValue(1, 1, 1, 1), Is.EqualTo(1));
	}

	[Test]
	public void ThrowsOnDifferentValues()
	{
		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(1, 2));
		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(1, 1, 2));
		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(1, 1, 1, 2));

		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(2, 1));
		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(2, 2, 1));
		Assert.Throws<RequirementException>(() => FF.RequireCommonValue(2, 2, 2, 1));
	}
}