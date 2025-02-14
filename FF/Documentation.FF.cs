namespace snns;

public static partial class FF
{
/*
	INTRODUCTION

	Class FF contains static methods for managing
	- strings
	- collections
	- nullability issues
	
	PUBLIC METHODS

	NAMING CONVENTIONS

	-	T/void RequireFactAbout(object o)
		-	throws unless the fact is true

	-	bool FactAbout(object o)
		-	Throws only if operations on o throw

	-	T MethodName(object o)
		-	returns T or throws

	-	T? TryMethodName(object o)
		-	returns T or null

	USEFUL AND USELESS

	FF introduces the concepts "Useful" and "Useless"
	-	An object o is useful iff it is not useless
	-	An object o is useless:
		-	if o is null
		-	if o is a string && string.IsNullOrWhitespace(o)
		-	if o is a collection with no useful items in it
*/
}