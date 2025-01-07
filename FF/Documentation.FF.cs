namespace snns;

public static partial class FF
{
/*
	FF contains a host of static methods for managing
	- strings
	- collections
	- nullability issues

	FF naming conventions:

	-	void AssertFactAbout(object o)
		-	throws unless the fact is true

	-	bool FactAbout(object o)
		-	Does not throw.
		-	Returns false if any underlying operation throws.

	-	T MethodName(object o)
		-	returns T or throws

	-	T? TryMethodName(object o)
		-	returns T or null

	FF introduces the concept "Useful" and "Useless"
	-	An object o is useful iff it is not useless
	-	An object o is useless if it resembles the result of deserializing pure
		whitespace:
		-	if o is null
		-	if o is a string && string.IsNullOrWhitespace(o)
		-	if o is a collection with no useful items in it
*/
}