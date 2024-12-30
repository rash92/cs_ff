using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException : Exception
{
	// ReSharper disable once ConvertToPrimaryConstructor
	public InvariantException(Reason reason)
	{
		_template = reason switch
		{
			Reason.IllegalNullable => NullTemplate,
			Reason.RecursionLimit => RecursionTemplate,
			Reason.EnumerationLimit => EnumerationTemplate,
			_ => DefaultMessage
		};
	}

	public enum Reason
	{
		RecursionLimit,
		IllegalNullable,
		EnumerationLimit
	}

	public override string Message => BuildMessage();

	public void PushNameOfCurrentContext(string memberName)
	{
		_newNames.Add(string.IsNullOrWhiteSpace(memberName) ? "__Anonymous__" : memberName);
		_message = null;
	}

	private string BuildMessage()
	{
		if (_message != null)
		{
			return _message;
		}

		var name = (_newNames.Count, _existingNames) switch
		{
			(1, null) => _newNames.Single(),
			(_, null) => ConcatNameElements(),
			(1, _) => string.Format(ConcatTemplate, _newNames.Single(), _existingNames),
			(_, _) => string.Format(ConcatTemplate, ConcatNameElements(), _existingNames)
		};

		var message = string.Format(_template, name);

		_existingNames = name;
		_message = message;
		_newNames.Clear();
		return _message;
	}

	private string ConcatNameElements()
	{
		if (_newNames.Count == 1)
		{
			return _newNames.Single();
		}

		var sb = new StringBuilder(_newNames.Count + _newNames.Sum(s => s.Length));
		foreach (var s in _newNames.AsEnumerable().Reverse().Take(_newNames.Count - 1))
		{
			sb.Append(s).Append('.');
		}

		sb.Append(_newNames.FirstOrDefault(""));

		return sb.ToString();
	}

	private const string DefaultMessage = "Unspecified invariant error {0}";
	private string? _message = DefaultMessage;

	private const string NullTemplate = "Non-nullable reference {0} is null";
	private const string RecursionTemplate = "Recursion limit reached in {0}";
	private const string EnumerationTemplate = "Enumeration limit reached in {0}";
	private readonly string _template;

	private const string ConcatTemplate = "{0}.{1}";
	private readonly List<string> _newNames = [];
	private string? _existingNames;
}