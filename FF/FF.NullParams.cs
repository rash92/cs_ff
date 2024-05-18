using System.Diagnostics.CodeAnalysis;

namespace snns;

public static partial class FF
{
	public static bool AllSet(params object?[]? objects)
	{
		if (objects == null) return false;

		foreach (var o in objects)
		{
			if (o == null) return false;
		}
		return true;
	}

	public static bool AnyNull([NotNullWhen(false)] object? o1)
	{
		return o1 == null;
	}

	public static bool AnyNull([NotNullWhen(false)] object? o1, [NotNullWhen(false)] object? oLast)
	{
		return o1 == null || oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null ||
			o2 == null ||
			oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? o4,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? o4,
		[NotNullWhen(false)] object? o5,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| o5 == null
			|| oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? o4,
		[NotNullWhen(false)] object? o5,
		[NotNullWhen(false)] object? o6,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| o5 == null
			|| o6 == null
			|| oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? o4,
		[NotNullWhen(false)] object? o5,
		[NotNullWhen(false)] object? o6,
		[NotNullWhen(false)] object? o7,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| o5 == null
			|| o6 == null
			|| o7 == null
			|| oLast == null;
	}

	public static bool AnyNull(
		[NotNullWhen(false)] object? o1,
		[NotNullWhen(false)] object? o2,
		[NotNullWhen(false)] object? o3,
		[NotNullWhen(false)] object? o4,
		[NotNullWhen(false)] object? o5,
		[NotNullWhen(false)] object? o6,
		[NotNullWhen(false)] object? o7,
		[NotNullWhen(false)] object? o8,
		[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| o5 == null
			|| o6 == null
			|| o7 == null
			|| o8 == null
			|| oLast == null;
	}

	public static bool AnyNull(
			[NotNullWhen(false)] object? o1,
			[NotNullWhen(false)] object? o2,
			[NotNullWhen(false)] object? o3,
			[NotNullWhen(false)] object? o4,
			[NotNullWhen(false)] object? o5,
			[NotNullWhen(false)] object? o6,
			[NotNullWhen(false)] object? o7,
			[NotNullWhen(false)] object? o8,
			[NotNullWhen(false)] object? o9,
			[NotNullWhen(false)] object? oLast)
	{
		return
			o1 == null
			|| o2 == null
			|| o3 == null
			|| o4 == null
			|| o5 == null
			|| o6 == null
			|| o7 == null
			|| o8 == null
			|| o9 == null
			|| oLast == null;
	}
}