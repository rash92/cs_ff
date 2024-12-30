using System;
using System.Net.WebSockets;
using NUnit.Framework.Internal;
using snns;

namespace Test_AssertNullInvariants
{
namespace DO_NOT_THROW_When
{
public class Required_members_ARE_SET_and
{
	[Test]
	public void Optional_members_ARE_SET()
	{
		Assert.DoesNotThrow(() =>
		{
			var t = TestObjects.Tree.FullyGrown();
			FF.AssertNullableInvariants(t);
		});
	}

	[Test]
	public void Optional_members_ARE_NOT_set()
	{
		Assert.DoesNotThrow(() =>
		{
			var t = TestObjects.Tree.Sapling();
			FF.AssertNullableInvariants(t);
		});
	}
}
}

namespace DO_THROW_When
{
public class OuterObject
{
	[Test]
	public static void IsNullReference()
	{
		Assert.Throws<InvariantException>(() =>
		{
			string s = null!;
			FF.AssertNullableInvariants(s);
		});
	}

	[Test]
	public static void IsNullStruct()
	{
		Assert.Throws<InvariantException>(() =>
		{
			int? i = null;
			FF.AssertNullableInvariants(i);
		});
	}

	[Test]
	public static void IsNull()
	{
		Assert.Throws<InvariantException>(() => { FF.AssertNullableInvariants((object)null!); });
	}
}

public class Required_members_are_NOT_SET_and
{
	[Test]
	public void Optional_members_ARE_SET()
	{
		Assert.Throws<InvariantException>(() =>
		{
			var t = TestObjects.Tree.Pruned();
			t.OptFld = TestObjects.Branch.FullyGrown();
			t.OptPrp = TestObjects.Branch.FullyGrown();
			FF.AssertNullableInvariants(t);
		});
	}

	[Test]
	public void Optional_members_ARE_NOT_set()
	{
		Assert.Throws<InvariantException>(() =>
		{
			var t = TestObjects.Tree.Pruned();
			FF.AssertNullableInvariants(t);
		});
	}
}
}

namespace FieldsAndProperties
{
public class Do_NOT_throw
{
	[Test]
	public void just_because_object_has_an_index_property()
	{
		var l = new List<int?>();
		l.Add(1);
		l.Add(null);

		Assert.DoesNotThrow(() => FF.AssertNullableInvariants(l));
	}
}

public class Do_NOT_throw_when_all_required_are_set_and
{
	[Test]
	public void optional_field_and_optional_property_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		Assert.DoesNotThrow(() => FF.AssertNullableInvariants(b));
	}

	[Test]
	public void optional_field_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.OptionalProperty = null;
		Assert.DoesNotThrow(() => FF.AssertNullableInvariants(b));
	}

	[Test]
	public void optional_property_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.OptionalField = null;
		Assert.DoesNotThrow(() => FF.AssertNullableInvariants(b));
	}
}

public class DO_THROW_When
{
	[Test]
	public void Required_property_is_not_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.RequiredProperty = null!;
		Assert.Throws<InvariantException>(() => FF.AssertNullableInvariants(b));
	}

	[Test]
	public void Required_field_is_not_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.RequiredField = null!;
		Assert.Throws<InvariantException>(() => FF.AssertNullableInvariants(b));
	}
}
}

public class TestObjects
{
	public class Bush
	{
		public Leaf RequiredProperty { get; set; } = new();
		public Leaf? OptionalProperty { get; set; } = new();
		public Leaf RequiredField = new();
		public Leaf? OptionalField = new();

		public static Bush FullyGrown() => new Bush();
	}

	public class Leaf
	{
	}

	public class Branch
	{
		public Leaf? OptPrp { get; set; }
		public Leaf ReqPrp { get; set; } = null!;
		public Leaf? OptFld = null;
		public Leaf ReqFld = null!;

		public static Branch FullyGrown()
		{
			return new Branch
			{
				OptPrp = new Leaf(),
				ReqPrp = new Leaf(),
				OptFld = new Leaf(),
				ReqFld = new Leaf()
			};
		}

		public static Branch Sapling()
		{
			return new Branch
			{
				ReqFld = new(),
				ReqPrp = new(),
			};
		}

		public static Branch Pruned()
		{
			return new Branch();
		}
	}

	public class Tree
	{
		public Branch? OptPrp { get; set; }
		public Branch ReqPrp { get; set; } = null!;
		public Branch? OptFld = null;
		public Branch ReqFld = null!;


		public static Tree FullyGrown()
		{
			return new Tree
			{
				OptPrp = Branch.FullyGrown(),
				ReqPrp = Branch.FullyGrown(),
				OptFld = Branch.FullyGrown(),
				ReqFld = Branch.FullyGrown()
			};
		}

		public static Tree Sapling()
		{
			return new Tree
			{
				ReqPrp = Branch.Sapling(),
				ReqFld = Branch.Sapling(),
			};
		}

		public static Tree Pruned()
		{
			return new Tree();
		}
	}


	[Test]
	public void Tree_Has04Branches_Has16Leaves_WhenComplete()
	{
		var t = Tree.FullyGrown();

		var branchCount = 0;

		if (t.ReqPrp != null) ++branchCount;
		if (t.ReqFld != null) ++branchCount;
		if (t.OptPrp != null) ++branchCount;
		if (t.OptFld != null) ++branchCount;

		Assert.That(branchCount, Is.EqualTo(4));

		var leafCount = 0;

		if (t.OptPrp?.OptPrp != null) ++leafCount;
		if (t.OptPrp?.OptFld != null) ++leafCount;
		if (t.OptPrp?.ReqPrp != null) ++leafCount;
		if (t.OptPrp?.ReqFld != null) ++leafCount;
		if (t.OptFld?.OptPrp != null) ++leafCount;
		if (t.OptFld?.OptFld != null) ++leafCount;
		if (t.OptFld?.ReqPrp != null) ++leafCount;
		if (t.OptFld?.ReqFld != null) ++leafCount;
		if (t.ReqPrp?.OptPrp != null) ++leafCount;
		if (t.ReqPrp?.OptFld != null) ++leafCount;
		if (t.ReqPrp?.ReqPrp != null) ++leafCount;
		if (t.ReqPrp?.ReqFld != null) ++leafCount;
		if (t.ReqFld?.OptPrp != null) ++leafCount;
		if (t.ReqFld?.OptFld != null) ++leafCount;
		if (t.ReqFld?.ReqPrp != null) ++leafCount;
		if (t.ReqFld?.ReqFld != null) ++leafCount;

		Assert.That(leafCount, Is.EqualTo(16));
	}

	[Test]
	public void Tree_Has02Branches_Has04Leaves_WhenPruned()
	{
		var t = Tree.Sapling();

		Assert.Null(t.OptPrp, "sapling tree should not have optional branch property");
		Assert.Null(t.OptFld, "sapling tree should not have optional branch field");

		Assert.NotNull(t.ReqPrp, "sapling tree should still have required branch property");
		Assert.NotNull(t.ReqFld, "sapling tree should still have required branch field");

		Assert.Null(t.ReqPrp.OptPrp);
		Assert.Null(t.ReqPrp.OptFld);
		Assert.Null(t.ReqFld.OptPrp);
		Assert.Null(t.ReqFld.OptFld);

		var branchCount = 0;

		if (t.ReqPrp != null) ++branchCount;
		if (t.ReqFld != null) ++branchCount;
		if (t.OptPrp != null) ++branchCount;
		if (t.OptFld != null) ++branchCount;

		Assert.That(branchCount, Is.EqualTo(2));

		var leafCount = 0;

		if (t.OptPrp?.OptPrp != null) ++leafCount;
		if (t.OptPrp?.OptFld != null) ++leafCount;
		if (t.OptPrp?.ReqPrp != null) ++leafCount;
		if (t.OptPrp?.ReqFld != null) ++leafCount;
		if (t.OptFld?.OptPrp != null) ++leafCount;
		if (t.OptFld?.OptFld != null) ++leafCount;
		if (t.OptFld?.ReqPrp != null) ++leafCount;
		if (t.OptFld?.ReqFld != null) ++leafCount;
		if (t.ReqPrp?.OptPrp != null) ++leafCount;
		if (t.ReqPrp?.OptFld != null) ++leafCount;
		if (t.ReqPrp?.ReqPrp != null) ++leafCount;
		if (t.ReqPrp?.ReqFld != null) ++leafCount;
		if (t.ReqFld?.OptPrp != null) ++leafCount;
		if (t.ReqFld?.OptFld != null) ++leafCount;
		if (t.ReqFld?.ReqPrp != null) ++leafCount;
		if (t.ReqFld?.ReqFld != null) ++leafCount;

		Assert.That(leafCount, Is.EqualTo(4));
	}

	[Test]
	public static void InvariantExceptionGathersNames()
	{
		var ie = new InvariantException();

		ie.PushNameOfCurrentContext("leaf");
		Assert.That(ie.Message, Is.EqualTo("Non-nullable reference leaf is null"));

		ie.PushNameOfCurrentContext("branch");
		Assert.That(ie.Message, Is.EqualTo("Non-nullable reference branch.leaf is null"));

		ie.PushNameOfCurrentContext("trunk");
		Assert.That(ie.Message, Is.EqualTo("Non-nullable reference trunk.branch.leaf is null"));
	}
}
}