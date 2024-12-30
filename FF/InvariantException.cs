using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snns;

public class InvariantException() : Exception
{
	public override string Message => _message ??= BuildMessage();

	public void PushNameOfCurrentContext(string memberName)
	{
		_names.Add(memberName.Length == 0 ? "__Anonymous__" : memberName);
		_message = null;
	}


	private string BuildMessage()
	{
		
//		if (_message != null) return _message;

		var sb = new StringBuilder(_names.Count + _names.Sum(s => s.Length));

		foreach (var s in _names.AsEnumerable().Reverse().Take(_names.Count - 1))
		{
			sb.Append(s).Append('.');
		}

		sb.Append(_names.FirstOrDefault(""));

		return string.Format(NullTemplate, sb);
	}

	private const string NullTemplate = "Non-nullable reference {0} is null";
	private readonly List<string> _names = new ();
	private string? _message;
}