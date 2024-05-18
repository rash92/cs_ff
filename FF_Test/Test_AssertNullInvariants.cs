using System.Linq;
using System.Reflection;
using snns;

namespace Test_FF;

public class Test_AssertNullInvariants
{
	public class Account
	{
		public User User { get; set; } = new();
	}

	public class User
	{
		public string GUID { get; set; } = null!;
	}

	[Test]
	public void CountFields()
	{
		var props = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		Assert.That(props, Has.Length.EqualTo(1));
		Assert.That(props.First().Name, Is.EqualTo("GUID"));
	}

	// [Test]
	// public void DoNotThrowWhenAllFieldsValid()
	// {
	// 	var a = new Account
	// 	{
	// 		User = new User { GUID = "PSEUDORANDOM" }
	// 	};
		
	// 	FF.AssertNullableInvariants(a);
		
	// 	Assert.Pass();
	// }

	// [Test]
	// public void PleaseThrowWhenAFieldIsInvalid()
	// {
	// 	const string jsonWithoutRequiredMemberGUID = """
	// 	                    {
	// 	                        "User" : {
	// 	                        }
	// 	                    }
	// 	                    """;

	// 	var account = System.Text.Json.JsonSerializer.Deserialize<Account>(jsonWithoutRequiredMemberGUID);
		
	// 	Assert.Throws<InvariantException>(() => FF.AssertNullableInvariants(account));
	// }
}