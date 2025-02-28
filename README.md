> # README

# Introduction

This project introduces `static class FF` to the namespace `snns`.

<!--

`FF` provides methods to solve recurring DRY problems.

The pitch: You can replace this:

```csharp
public void YourMethod()
{
    var messages = _messageBus.Read();
    if(messages == null)
      return;
  
    messages = messages
                .Where( m => !string.IsNullOrWhitespace(m) )
                .Select( m => m! )
                .ToList();
    if( messages.Count == 0 )
        return;
    else
        Process(messages);
}
```

With this:

```csharp
public void YourMethod()
{
    var messages = FF.WhereUseful(_messageBus.read())

    if(messages == null || messages.Count == 0)
        return;
    else
        Process(messages);
}
```


# Roadmap

In no particular order:

- Set up CI/CD
  - ~~Test on push/PR~~ DONE
- Release management
  - Establish reliable release branches
  - Upload releases to nuget.org on command
  - Write scripts to automate most of this
- Expand
  - Transfer more methods from my private repos to here
  - And their tests
  - Transfer my StrongType library from private to public
- Usability
  - Change naming pattern to make it more clear how a method works
    - Does it throw? Return null? Return false?
  - Add extension method equivalents for single-argument methods.

-->