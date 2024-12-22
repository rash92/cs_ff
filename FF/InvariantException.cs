using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException : Exception
{
	public enum Reason
	{
		HasNullMember,
		HasNonEnumerableIndexParam
	}
	
	public InvariantException(string memberName, Reason reason)
	{
		Add(memberName);
		_reason = reason;
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
		var (prefix, postfix) = _reason == Reason.HasNullMember
			? (NullPrefix, NullPostfix)
			: (IndexPrefix, IndexPostfix);
		
		var sb = new StringBuilder(prefix.Length +
		                           postfix.Length +
		                           _names.Sum(n => n.Length) +
		                           _names.Count -
		                           1); //fencepost - period between each name, 3 names => 2 periods

		sb.Append(NullPrefix);

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

		sb.Append(NullPostfix);
		
		return sb.ToString();
	}

	private const string NullPrefix = "Non-nullable reference ";
	private const string NullPostfix = " is null.";
	private const string IndexPrefix = "Index property ";
	private const string IndexPostfix = " cannot be enumerated.";
	private readonly List<string> _names = new List<string>(1);
	private Reason _reason;
}