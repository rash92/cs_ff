using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace snns;

public static partial class FF
{
	public static bool IsUseful([NotNullWhen(true)] object? obj)
	{
		return obj != null;
	}

	public static bool IsUseful([NotNullWhen(true)] string? str)
	{
		return !string.IsNullOrWhiteSpace(str);
	}

	public static bool IsUseful<T>([NotNullWhen(true)] T? t) where T : class
	{
		return t != null;
	}
	
	public static bool IsUseful<T>([NotNullWhen(true)] T? t) where T : struct
	{
		return t.HasValue;
	}
	
	public static bool IsUseful<T>([NotNullWhen(true)] IEnumerable<T?>? t) where T : class
	{
		return (t?.Any(IsUseful)).GetValueOrDefault(false);
	}

	public static bool IsUseful<T>([NotNullWhen(true)] IEnumerable<T?>? t) where T : struct
	{
		return (t?.Any(IsUseful)).GetValueOrDefault(false);
	}
}