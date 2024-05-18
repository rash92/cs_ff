namespace snns;

public static partial class FF
{
	public static bool SameElements<T>(IReadOnlyCollection<T> left,
	                                   IReadOnlyCollection<T> right)
		where T : IEquatable<T>, IComparable<T>
	{
		if (left.Count != right.Count)
			return false;

		using var lItr = left.Order().GetEnumerator();
		using var rItr = right.Order().GetEnumerator();

		while (lItr.MoveNext() && rItr.MoveNext())
			if (!lItr.Current.Equals(rItr.Current))
				return false;

		return true;
	}
}