using System;
using System.Collections.Generic;
using NUnit.Framework;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
#endif

namespace ServiceStack.Text.Tests
{
	[TestFixture]
#if NETCF
    [TestClass]
#endif
	public class AnonymousTypes
		: TestBase
	{
#if NETCF
        [TestMethod]
        [Ignore] // .NET CF does not completely support anonymous type
#endif
        [Test]
        public void Can_serialize_anonymous_types()
		{
		    System.Diagnostics.Debugger.Break();
			Serialize(new { Id = 1, Name = "Name", IntList = new[] { 1, 2, 3 } }, /*includeXml:*/ false); // xmlserializer cannot serialize anonymous types.
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_serialize_anonymous_type_and_read_as_string_Dictionary()
		{
			var json = JsonSerializer.SerializeToString(
				new { Id = 1, Name = "Name", IntList = new[] { 1, 2, 3 } });

			Console.WriteLine("JSON: " + json);

			var map = JsonSerializer.DeserializeFromString<Dictionary<string, string>>(json);

			Console.WriteLine("MAP: " + map.Dump());
		}

        public class TestObj
        {
            public string Title1 { get; set; }
            public object Title2 { get; set; }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Escapes_string_in_object_correctly()
        {
            const string expectedValue = @"a\nb";
            string json = string.Format(@"{{""Title1"":""{0}"",""Title2"":""{0}""}}", expectedValue);

            var value = JsonSerializer.DeserializeFromString<TestObj>(json);

            value.PrintDump();

            Assert.That(value.Title1, Is.EqualTo(value.Title2.ToString()));
        }
	}

}