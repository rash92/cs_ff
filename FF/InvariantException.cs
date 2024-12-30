using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException : Exception
{
	public override string Message => BuildMessage();

	public void PushNameOfCurrentContext(string memberName)
	{
		_nameElements.Add(string.IsNullOrWhiteSpace(memberName) ? "__Anonymous__" : memberName);
		_message = null;
	}

	private string BuildMessage()
	{
		if (_message != null)
		{
			return _message;
		}

		var name = (_nameElements.Count, _existingName) switch
		{
			(1, null) => _nameElements.Single(),
			(_, null) => ConcatNameElements(),
			(1, _) => string.Format(ConcatTemplate, _nameElements.Single(), _existingName),
			(_, _) => string.Format(ConcatTemplate, ConcatNameElements(), _existingName)
		};

		var message = string.Format(_template, name);

		_existingName = name;
		_message = message;
		_nameElements.Clear();
		return _message;
	}

	private string ConcatNameElements()
	{
		if (_nameElements.Count == 1)
		{
			return _nameElements.Single();
		}

		var sb = new StringBuilder(_nameElements.Count + _nameElements.Sum(s => s.Length));
		foreach (var s in _nameElements.AsEnumerable().Reverse().Take(_nameElements.Count - 1))
		{
			sb.Append(s).Append('.');
		}

		sb.Append(_nameElements.FirstOrDefault(""));

		return sb.ToString();
	}

	private const string DefaultMessage = "Non-nullable reference is null";
	private string? _message = DefaultMessage;

	private const string NullTemplate = "Non-nullable reference {0} is null";
	private string _template = NullTemplate;

	private const string ConcatTemplate = "{0}.{1}";
	private readonly List<string> _nameElements = [];
	private string? _existingName;
}