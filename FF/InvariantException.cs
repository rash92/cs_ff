using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException : Exception
{
	public enum Reason
	{
		HasNull,
		HasNonEnumerableIndexProperty,
		HasMultipleIndexProperties, //impossible in C# but possible in e.g. VB.Net so foreign types could have 2+
	}

	public InvariantException(string memberName, Reason reason)
	{
		PushNameOfCurrentContext(memberName);
		_reason = reason;
	}

	public void PushNameOfCurrentContext(string memberName)
	{
		_names.Add(memberName.Length == 0 ? "__Anonymous__" : memberName);
		_message = null;
	}

	public override string Message => BuildMessage();

	private string BuildMessage()
	{
		if (_message != null) return _message;

		var sb = new StringBuilder(_names.Count + _names.Sum(s => s.Length));

		foreach (var s in _names.AsEnumerable().Reverse().Take(_names.Count - 1))
		{
			sb.Append(s).Append('.');
		}

		sb.Append(_names.FirstOrDefault(""));

		var template = _reason switch
		{
			Reason.HasNull => NullTemplate,
			Reason.HasNonEnumerableIndexProperty => IndexTemplate,
			_ => DefaultTemplate
		};

		return string.Format(template, sb);
	}

	private const string NullTemplate = "Non-nullable reference {0} is null";
	private const string IndexTemplate = "Index property {0} cannot be enumerated";
	private const string DefaultTemplate = "unspecified error handling {0}";

	private const string NullPrefix = "Non-nullable reference ";
	private const string NullPostfix = " is null.";
	private const string IndexPrefix = "Index property ";
	private const string IndexPostfix = " cannot be enumerated.";
	private readonly List<string> _names = new List<string>();
	private Reason _reason;
	private string? _message = null;
}