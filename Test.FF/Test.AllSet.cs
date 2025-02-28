using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using snns;

namespace Test_AllSet;

public class AllSet
{
	[Test]
	public void Accepts_the_empty_list()
	{
		Assert.That(FF.AllSet(), Is.True);
	}

	[Test]
	public void Accepts_single_elements()
	{
		Assert.That(FF.AllSet("hey"), Is.True);
		Assert.That(FF.AllSet(null), Is.False);
		Assert.That(FF.AllSet(new List<char>()), Is.True);
	}

	[Test]
	public void Returns_true_when_no_nulls_are_present_in_lists_of_varying_size()
	{
		Assert.That(FF.AllSet("Hello"), Is.True);
		Assert.That(FF.AllSet("Hello",1), Is.True);
		Assert.That(FF.AllSet("Hello",1,new List<int>()), Is.True);
		Assert.That(FF.AllSet("Hello",1,new List<int>(),DateTime.Now), Is.True);
		Assert.That(FF.AllSet("Hello",1,new List<int>(),DateTime.Now,"world!"), Is.True);
	}

	[Test]
	public void Returns_false_no_for_null_in_any_position()
	{
		Assert.That(FF.AllSet("1", "2", "3", "4", "5", "6", "7", "8", "9"), Is.True);

		Assert.That(FF.AllSet(null, "2", "3", "4", "5", "6", "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", null, "3", "4", "5", "6", "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", null, "4", "5", "6", "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", null, "5", "6", "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", "4", null, "6", "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", "4", "5", null, "7", "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", "4", "5", "6", null, "8", "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", "4", "5", "6", "7", null, "9"), Is.False);
		Assert.That(FF.AllSet("1", "2", "3", "4", "5", "6", "7", "8", null), Is.False);
	}
}

public class AnyNull
{
	[Test]
	public void DefaultTest()
	{
		Assert.That(FF.AnyNull(null));
		Assert.That(FF.AnyNull(null,null));
		Assert.That(FF.AnyNull(null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null,null,null,null,null));
		Assert.That(FF.AnyNull(null,null,null,null,null,null,null,null,null,null));
		
		Assert.That(!FF.AnyNull(""));
		Assert.That(!FF.AnyNull("",""));
		Assert.That(!FF.AnyNull("","",""));
		Assert.That(!FF.AnyNull("","","",""));
		Assert.That(!FF.AnyNull("","","","",""));
		Assert.That(!FF.AnyNull("","","","","",""));
		Assert.That(!FF.AnyNull("","","","","","",""));
		Assert.That(!FF.AnyNull("","","","","","","",""));
		Assert.That(!FF.AnyNull("","","","","","","","",""));
		Assert.That(!FF.AnyNull("","","","","","","","","",""));
		
		Assert.That(FF.AnyNull(null,""));
		Assert.That(FF.AnyNull(null,"",""));
		Assert.That(FF.AnyNull(null,"","",""));
		Assert.That(FF.AnyNull(null,"","","",""));
		Assert.That(FF.AnyNull(null,"","","","",""));
		Assert.That(FF.AnyNull(null,"","","","","",""));
		Assert.That(FF.AnyNull(null,"","","","","","",""));
		Assert.That(FF.AnyNull(null,"","","","","","","",""));
		Assert.That(FF.AnyNull(null,"","","","","","","","",""));
		
		Assert.That(FF.AnyNull("",null));
		Assert.That(FF.AnyNull("","",null));
		Assert.That(FF.AnyNull("","","",null));
		Assert.That(FF.AnyNull("","","","",null));
		Assert.That(FF.AnyNull("","","","","",null));
		Assert.That(FF.AnyNull("","","","","","",null));
		Assert.That(FF.AnyNull("","","","","","","",null));
		Assert.That(FF.AnyNull("","","","","","","","",null));
		Assert.That(FF.AnyNull("","","","","","","","","",null));
	}
}