namespace snns;

public static partial class FF
{
	/// <summary>
	/// Uses .Equals to determine if any 'outer' elements are in the 'inner' collection.
	/// 
	/// Worst-case performance: O(outer.count * inner.count).
	///
	/// Robust against null collections and null elements in the collection.
	///
	/// May throw: Only if T.Equals(T) can throw.
	/// </summary>
	public static bool ContainsAnyOf<T>(IReadOnlyCollection<T>? outer, IReadOnlyCollection<T>? inner)
	{
		if (outer == null) return false;
		if (outer.Count == 0) return false;
		if (inner == null) return false;
		if (inner.Count == 0) return false;

		foreach (var o in outer)
		{
			foreach (var i in inner)
			{
				if (o is null && i is null) return true;
				if (o is null) continue;
				if (i is null) continue;
				if (o.Equals(i)) return true;
			}
		}

		return false;
	}
}