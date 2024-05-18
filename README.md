# README

- [README](#readme)
  - [Introduction](#introduction)
  - [Method naming idioms](#method-naming-idioms)


## Introduction

The static class FF provides a number of static methods to solve problems that come up often, e.g. "Throw an exception on bad input."

```csharp
	public void YourMethod(IList<string> words)
	{
		FF.AssertAllUseful(words);
		//use "words" here without worrying about nulls
	}
```

Where relevant, FF also introduces extension methods that work the same,
prefixed with "FF" to namespace pollution:

```csharp
	public YourClass(IEnumerable<string> words)
	{
		words.FFAssertAllUseful();
	}
```

## Method naming idioms

<< to be continued >>