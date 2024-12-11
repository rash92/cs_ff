using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException : Exception
{
	public InvariantException(string memberName)
	{
		Add(memberName);
	}

	public void AddNameOfCurrentContext(string memberName)
	{
		Add(memberName);
	}

	private void Add(string memberName)
	{
		_names.Add(memberName == "" ? "__Anonymous__" : memberName);
	}

	public override string Message => ConCat();

	private string ConCat()
	{
		var sb = new StringBuilder(PostFix.Length +
		                           Prefix.Length +
		                           _names.Sum(n => n.Length) +
		                           _names.Count -
		                           1); //fencepost - period between each name, 3 names => 2 periods

		sb.Append(Prefix);

		// pure garbage loop but we need to add names in reverse while adding periods but only in between.
		sb.Append(_names.Last());
		for (var i = _names.Count - 2; 0 <= i; --i)
		{
			sb.Append('.').Append(_names[i]);
		}
		// I guess we could also just add a period after every name, then remove the last period? But that would
		// just move the ugliness somewhere else. There is probably a string.ReverseConcat or something like
		// that but then it wouldn't know about the pre/postfix OR it would also concat those with periods in
		// between which is ALSO not what we want.

		sb.Append(PostFix);
		
		return sb.ToString();
	}

	private const string PostFix = " is null.";
	private readonly List<string> _names = new List<string>(1);
	private const string Prefix = "Non-nullable reference ";
}