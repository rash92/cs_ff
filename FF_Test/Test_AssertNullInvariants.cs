using System.Linq;
using snns;

namespace Test_AssertNullInvariants
{
public class RecursionAndEnumeration
{
	public enum Behavior
	{
		Throw,
		DontThrow
	}

	[Test]
	// empty collections never over-enumerate
	[TestCase(0, 0, Behavior.DontThrow)]
	[TestCase(0, 1, Behavior.DontThrow)]
	[TestCase(0, long.MaxValue, Behavior.DontThrow)]
	// fencepost error near 1 item
	[TestCase(1, 0, Behavior.Throw)]
	[TestCase(1, 1, Behavior.DontThrow)]
	[TestCase(1, 2, Behavior.DontThrow)]
	[TestCase(1, long.MaxValue, Behavior.DontThrow)]
	// general case
	[TestCase(100, 0, Behavior.Throw)]
	[TestCase(100, 1, Behavior.Throw)]
	[TestCase(100, 99, Behavior.Throw)]
	[TestCase(100, 100, Behavior.DontThrow)]
	[TestCase(100, 101, Behavior.DontThrow)]
	[TestCase(100, long.MaxValue, Behavior.DontThrow)]
	public void ThrowsIfCollectionHasMoreItemsThanEnumerationLimit(int items, long enumerationLimit, Behavior behavior)
	{
		var collection = Enumerable.Range(0, items);

		switch (behavior)
		{
			case Behavior.Throw:
			{
				Assert.Throws<InvariantException>(
					() => FF.RequireOnlyNullablesAreNull(collection, enumerationLimit: enumerationLimit));
				Assert.That(FF.OnlyNullablesAreNull(collection, enumerationLimit: enumerationLimit), Is.False);
				break;
			}
			case Behavior.DontThrow:
			{
				Assert.DoesNotThrow(
					() => FF.RequireOnlyNullablesAreNull(collection, enumerationLimit: enumerationLimit));
				Assert.That(FF.OnlyNullablesAreNull(collection, enumerationLimit: enumerationLimit), Is.True);
				break;
			}
			default:
			{
				Assert.Fail();
				break;
			}
		}
	}

	public class Wrapper
	{
		public Wrapper? W { get; set; }
	}

	[Test]
	public void ThrowsIfObjectHasMoreLinksThanTheRecursionLimit()
	{
		Wrapper w = new Wrapper();

		for (var i = 0; i < 5; i++)
		{
			var wrap = new Wrapper();
			wrap.W = w;
			w = wrap;
		}

		Assert.NotNull( /**/ w.W);
		Assert.NotNull( /**/ w.W!.W);
		Assert.NotNull( /**/ w.W!.W!.W);
		Assert.NotNull( /**/ w.W!.W!.W!.W);
		Assert.NotNull( /**/ w.W!.W!.W!.W!.W);
		Assert.Null( /*****/ w.W!.W!.W!.W!.W!.W);

		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(w));
		Assert.That(FF.OnlyNullablesAreNull(w));

		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 1));
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 2));
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 3));
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 4));
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 5));
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(w, recursionLimit: 6));

		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 1), Is.False);
		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 2), Is.False);
		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 3), Is.False);
		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 4), Is.False);
		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 5), Is.True);
		Assert.That(FF.OnlyNullablesAreNull(w, recursionLimit: 6), Is.True);

		// create a cyclic graph
		w.W = w;

		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(w));
		Assert.That(FF.OnlyNullablesAreNull(w));
	}

	public class RecursiveEnumerativeSpaghetti
	{
		public List<RecursiveEnumerativeSpaghetti>? List { get; set; }
		public string String { get; set; } = "required";
	}

	public RecursiveEnumerativeSpaghetti CookSpaghetti(bool breakInvariant = false)
	{
		var spaghetti1 = new RecursiveEnumerativeSpaghetti();
		spaghetti1.List = new();

		var spaghetti2 = new RecursiveEnumerativeSpaghetti();
		spaghetti2.List = new();

		spaghetti1.List.Add(spaghetti2);
		spaghetti2.List.Add(spaghetti1);

		foreach (var i in Enumerable.Range(0, 100))
		{
			var s1a = new RecursiveEnumerativeSpaghetti { List = [] };
			var s1b = new RecursiveEnumerativeSpaghetti { List = [] };
			var s2a = new RecursiveEnumerativeSpaghetti { List = [] };
			var s2b = new RecursiveEnumerativeSpaghetti { List = [] };

			s1a.List.Add(s1a);
			s1a.List.Add(s1b);
			s1a.List.Add(s2a);
			s1a.List.Add(s2b);
			s1a.List.Add(spaghetti1.List.First());
			s1a.List.Add(spaghetti1.List.Last());
			s1a.List.Add(spaghetti2.List.First());
			s1a.List.Add(spaghetti2.List.Last());
			spaghetti1.List.Add(s1a);
			spaghetti2.List.Add(s1a);

			s1b.List.Add(s1a);
			s1b.List.Add(s1b);
			s1b.List.Add(s2a);
			s1b.List.Add(s2b);
			s1b.List.Add(spaghetti1.List.First());
			s1b.List.Add(spaghetti1.List.Last());
			s1b.List.Add(spaghetti2.List.First());
			s1b.List.Add(spaghetti2.List.Last());
			spaghetti1.List.Add(s1b);
			spaghetti2.List.Add(s1b);

			s2a.List.Add(s1a);
			s2a.List.Add(s1b);
			s2a.List.Add(s2a);
			s2a.List.Add(s2b);
			s2a.List.Add(spaghetti1.List.First());
			s2a.List.Add(spaghetti1.List.Last());
			s2a.List.Add(spaghetti2.List.First());
			s2a.List.Add(spaghetti2.List.Last());
			spaghetti1.List.Add(s2a);
			spaghetti2.List.Add(s2a);

			s2b.List.Add(s1a);
			s2b.List.Add(s1b);
			s2b.List.Add(s2a);
			s2b.List.Add(s2b);
			s2b.List.Add(spaghetti1.List.First());
			s2b.List.Add(spaghetti1.List.Last());
			s2b.List.Add(spaghetti2.List.First());
			s2b.List.Add(spaghetti2.List.Last());
			spaghetti1.List.Add(s2b);
			spaghetti2.List.Add(s2b);

			if (i == 50 && breakInvariant)
			{
				s2b.String = null!;
			}
		}

		var evenMoreSpaghetti = new RecursiveEnumerativeSpaghetti
		{
			List = [spaghetti1, spaghetti2]
		};

		return evenMoreSpaghetti;
	}

	private const int ZeroRecursion = 0;
	private const int FreeRecursion = int.MaxValue;
	private const long ZeroEnumeration = 0;
	private const long FreeEnumeration = long.MaxValue;
	private const bool BreakInvariants = true;
	private const bool KeepsInvariants = false;

	[Test]
	//Bad invariants:
	[TestCase(ZeroRecursion, ZeroEnumeration, BreakInvariants, Behavior.Throw)] // fully constrained => failure
	[TestCase(FreeRecursion, ZeroEnumeration, BreakInvariants, Behavior.Throw)] // constrained enumeration => failure
	[TestCase(ZeroRecursion, FreeEnumeration, BreakInvariants, Behavior.Throw)] // constrained recursion => failure
	[TestCase(FreeRecursion, FreeEnumeration, BreakInvariants, Behavior.Throw)] // no constraints=> still failure
	//Good invariants:
	[TestCase(ZeroRecursion, ZeroEnumeration, KeepsInvariants, Behavior.Throw)] // fully constrained => failure
	[TestCase(FreeRecursion, ZeroEnumeration, KeepsInvariants, Behavior.Throw)] // constrained enumeration => failure
	[TestCase(ZeroRecursion, FreeEnumeration, KeepsInvariants, Behavior.Throw)] // constrained recursion => failure
	[TestCase(FreeRecursion, FreeEnumeration, KeepsInvariants, Behavior.DontThrow)] // no constraints=> success
	public void CheckInvariantsInRecursiveEnumerativeSpaghetti(int recursions,
	                                                           long enumerations,
	                                                           bool invariants,
	                                                           Behavior behavior)
	{
		var spaghetti = CookSpaghetti(invariants);

		switch (behavior)
		{
			case Behavior.Throw:
			{
				Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(
					                                  spaghetti,
					                                  recursions,
					                                  enumerations));

				Assert.That(FF.OnlyNullablesAreNull(spaghetti,
				                                    recursions,
				                                    enumerations),
				            Is.False);
				break;
			}
			case Behavior.DontThrow:
			{
				Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(
					                    spaghetti,
					                    recursions,
					                    enumerations));

				Assert.That(FF.OnlyNullablesAreNull(spaghetti,
				                                    recursions,
				                                    enumerations),
				            Is.True);
				break;
			}
			default:
			{
				Assert.Fail();
				break;
			}
		}
	}
}


namespace DO_NOT_THROW_When
{
public class Required_members_ARE_SET_and
{
	[Test]
	public void Optional_members_ARE_SET()
	{
		var t = TestObjects.Tree.FullyGrown();
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(t));
		Assert.That(FF.OnlyNullablesAreNull(t));
	}

	[Test]
	public void Optional_members_ARE_NOT_set()
	{
		var t = TestObjects.Tree.Sapling();
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(t));
		Assert.That(FF.OnlyNullablesAreNull(t));
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
		string s = null!;
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(s));
		Assert.That(!FF.OnlyNullablesAreNull(s));
	}

	[Test]
	public static void IsNullStruct()
	{
		int? i = null;
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(i));
		Assert.That(!FF.OnlyNullablesAreNull(i));
	}

	[Test]
	public static void IsNull()
	{
		Assert.Throws<InvariantException>(() => { FF.RequireOnlyNullablesAreNull((object)null!); });
		Assert.That(!FF.OnlyNullablesAreNull((object)null!));
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
			FF.RequireOnlyNullablesAreNull(t);
		});
	}

	[Test]
	public void Optional_members_ARE_NOT_set()
	{
		Assert.Throws<InvariantException>(() =>
		{
			var t = TestObjects.Tree.Pruned();
			FF.RequireOnlyNullablesAreNull(t);
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

		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(l));
	}
}

public class Do_NOT_throw_when_all_required_are_set_and
{
	[Test]
	public void optional_field_and_optional_property_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(b));
	}

	[Test]
	public void optional_field_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.OptionalProperty = null;
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(b));
	}

	[Test]
	public void optional_property_is_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.OptionalField = null;
		Assert.DoesNotThrow(() => FF.RequireOnlyNullablesAreNull(b));
	}
}

public class DO_THROW_When
{
	[Test]
	public void Required_property_is_not_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.RequiredProperty = null!;
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(b));
	}

	[Test]
	public void Required_field_is_not_set()
	{
		var b = TestObjects.Bush.FullyGrown();
		b.RequiredField = null!;
		Assert.Throws<InvariantException>(() => FF.RequireOnlyNullablesAreNull(b));
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
		var ie = new InvariantException(InvariantException.Reason.IllegalNullable);

		Assert.That(ie.Message, Is.EqualTo("Unspecified invariant error {0}"));

		ie.PushNameOfCurrentContext("Leaf");
		Assert.That(ie.Message, Is.EqualTo("Required reference Leaf is null"));

		ie.PushNameOfCurrentContext("Branch");
		Assert.That(ie.Message, Is.EqualTo("Required reference Branch.Leaf is null"));

		ie.PushNameOfCurrentContext("Trunk");
		Assert.That(ie.Message, Is.EqualTo("Required reference Trunk.Branch.Leaf is null"));
	}
}
}