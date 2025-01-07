namespace snns;


public static partial class FF
{
/*
	FF contains a host of static methods for managing
	- strings
	- collections
	- nullability issues

	FF naming conventions:
	-	void AssertXYZ(object o)
		-	will throw unless o lives up to XYZ

	-	bool IsXYZ(object o)
		-	returns true if o lives up to XYZ
		-	Throws if, and only if, an operation on o can throw.
	
	-	

	FF introduces the concept "Useful" and "Useless"
	-	An object o is useful iff it is not useless
	-	An object o is useless if it resembles the result of deserializing pure
		whitespace:
		-	if o is null
		-	if o is a string && string.IsNullOrWhitespace(o)
		-	if o is a collection with no useful items in it
	-	The methods named "IsUseful"

*/
}