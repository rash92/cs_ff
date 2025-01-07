using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace snns;

public static partial class FF
{
	/// <summary>
	/// IsUseful(s) == !string.IsNullOrWhiteSpace(s)
	/// </summary>
	/// <param name="s">Null or any string</param>
	/// <returns>
	/// True when not false.<br/>
	/// False when s is null, empty, or contains only whitespace.
	/// </returns>
	[Pure]
	public static bool IsUseful([NotNullWhen(true)] string? s)
	{
		return !string.IsNullOrWhiteSpace(s);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="collection"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	private static bool IsUseful<T>([NotNullWhen(true)] IReadOnlyCollection<T>? collection)
	{
		if (collection == null || collection.Count == 0)
			return false;
		//TODO
		throw new NotImplementedException();
	}
}