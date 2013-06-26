using NUnit.Framework;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
#endif

namespace ServiceStack.Text.Tests.JsonTests
{
#if NETCF
    [TestClass]
#endif
    [TestFixture]
    public class AnonymousDeserializationTests
		: TestBase
	{
		private class Item
		{
			public int IntValue { get; set; }
			public string StringValue { get; set; }

			public static Item Create()
			{
				return new Item { IntValue = 42, StringValue = "Foo" };
			}
		}

#if NETCF
        [TestMethod]
        [Ignore] // .NET CF does not fully support anonymous types
#endif
        [Test]
		public void Can_deserialize_to_anonymous_type()
		{
			var original = Item.Create();
			var json = JsonSerializer.SerializeToString(original);
			
			var item = DeserializeAnonymousType(new { IntValue = default(int), StringValue = default(string) }, json);

			Assert.That(item.IntValue, Is.EqualTo(42));
			Assert.That(item.StringValue, Is.EqualTo("Foo"));
		}

		private static T DeserializeAnonymousType<T>(T template, string json) where T : class
		{
			TypeConfig<T>.EnableAnonymousFieldSetters = true;
			return JsonSerializer.DeserializeFromString(json, template.GetType()) as T;
		}
	}
}