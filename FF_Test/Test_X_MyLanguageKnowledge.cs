using System;

namespace Test_FF;

public class Test_X_MyLanguageKnowledge
{
	[Test, Explicit]
	public void WhatEvenArePropertyInfos()
	{
		var chars = new char[32];

		var propInfos = chars.GetType().GetProperties();

		Assert.Pass(); //"The name 'propInfos' does not exist in the current context" ... excuse me?
	}

	public class Outer
	{
		// public struct Inner
		// {
		// 	public unsafe fixed char Array[4]{
		// 		'a','b','c','d'
		// 	};
		//
		// 	public Inner()
		// 	{
		// 		unsafe
		// 		{
		// 			Array  ;
		// 		}
		// 	}
		//
		// 	public int Property { get; set; } = 0;
		// }
		//
		// public Inner inner { get; set; }
	}

	[Test, Explicit]
	public void SeriouslyWhatIsIt()
	{
		// var outer = new Outer();
		//
		// var propInfos = outer.GetType().GetProperties();
		//
		// Assert.Pass();
	}

	[Test, Explicit]
	public void IsItJustStrings()
	{
		// ReSharper disable once ConvertToConstant.Local
		var s = "PSEUDORANDOM";
		Console.WriteLine(s);

		var t = s.GetType();
		Console.WriteLine(t.ToString());

		var p = t.GetProperties(System.Reflection.BindingFlags.Public |
		                        System.Reflection.BindingFlags.NonPublic |
		                        System.Reflection.BindingFlags.Instance);
		Console.WriteLine(p.ToString());

		Assert.That(p, Is.Not.EqualTo(null));
	}

	[Test, Explicit]
	public void WhatEvenIsADelegate()
	{
		//I mean it's a function pointer yes but like.
		//How.
		var t = new Publisher().GetType();
		var m = t.GetMembers();
		// Ok a delegate inside a class is *not* a member
		Assert.That(m, Is.Not.EqualTo(null)); // just
	}

	//From: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event
	public class SampleEventArgs
	{
		public SampleEventArgs(string text)
		{
			Text = text;
		}

		public string Text { get; } // readonly
	}

	//From: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event
	public class Publisher
	{
		public delegate void SampleEventHandler(object sender, SampleEventArgs e);

		public event SampleEventHandler? SampleEvent;

		protected virtual void RaiseSampleEvent()
		{
			// Raise the event in a thread-safe manner using the ?. operator.
			SampleEvent?.Invoke(this, new SampleEventArgs("Hello"));
		}
	}
}