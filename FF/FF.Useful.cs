using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace snns;

public static partial class FF
{
	public static void RequireUseful([NotNullWhen(true)]object? obj)
	{
		if (IsUseful(obj))
			return;
		var ex = new InvariantException(InvariantException.Reason.NotUseful);
		ex.PushNameOfCurrentContext(obj?.GetType().Name ?? "null");
		throw ex;
	}

	public static bool IsUseful([NotNullWhen(true)]object? obj)
	{
		return obj switch
		{
			null => false,
			string str => IsUseful(str),
			IEnumerable ie => IsUseful(ie),
			_ => true,
		};
	}

	public static bool IsUseful([NotNullWhen(true)]IEnumerable? ie)
	{
		return ie?.Cast<object?>()?.Any(IsUseful) ?? false;
	}

	public static bool IsUseful([NotNullWhen(true)]string? str)
	{
		return !string.IsNullOrWhiteSpace(str);
	}
}