using System.Text;

namespace snns;

public static partial class FF
{
	public static T RequireCommonValue<T>(IEnumerable<T> ts) where T : IEquatable<T>
	{
		var array = ts.ToArray();

		return array.Length switch
		{
			0 => throw new RequirementException("Cannot select common value from zero values"),
			1 => array.First(),
			_ => RequireCommonValue(array.First(), array.Skip(1).First(), array.Skip(2).ToArray())
		};
	}

	public static T RequireCommonValue<T>(T first, T second, params T[] rest) where T : IEquatable<T>
	{
		bool common = first.Equals(second);

		foreach (var r in rest)
		{
			if (!common) break;
			if (!first.Equals(r)) common = false;
		}

		if (!common)
			throw new RequirementException(CreateRequireCommonValueMessage(first, second, rest));

		return first;
	}

	public static string CreateRequireCommonValueMessage<T>(T first, T second, params T[] rest)
	{
		var stringBuilder = new StringBuilder("Elements required to all be the same but they were not. Values were: {")
			.Append(first)
			.Append("},{")
			.Append(second);

		foreach (var r in rest)
		{
			stringBuilder
				.Append("},{")
				.Append(r);
		}

		return stringBuilder.Append('}').ToString();
	}
}