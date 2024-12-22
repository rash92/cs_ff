namespace snns;

public static partial class FF
{
	public static (IEnumerable<T> True, IEnumerable<T> False) Split<T>(
		IReadOnlyCollection<T> coll,
		Predicate<T> pred)
	{
		return (coll.Where(c => pred(c)), coll.Where(c => !pred(c)));
	}
}