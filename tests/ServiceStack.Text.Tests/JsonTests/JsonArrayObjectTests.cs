using System.Collections.Generic;
using NUnit.Framework;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using ClassCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute;
using TestCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
#endif

namespace ServiceStack.Text.Tests.JsonTests
{
#if NETCF
    [TestClass]
#endif
    [TestFixture]
    public class JsonArrayObjectTests
    {

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_int_array() 
		{
			var array = new [] {1,2};
			Assert.That(JsonSerializer.SerializeToString(array), Is.EqualTo("[1,2]"));
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_empty_array()
        {
            Assert.That(JsonArrayObjects.Parse("[]"), Is.Empty);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_empty_array_with_tab()
        {
            Assert.That(JsonArrayObjects.Parse("[\t]"), Is.Empty);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_array_with_null()
        {
            Assert.That(JsonArrayObjects.Parse("[null]"), Is.Empty);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_array_with_nulls()
        {
            Assert.That(JsonArrayObjects.Parse("[null,null]"), Is.Empty);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_empty_array_with_whitespaces()
        {
            Assert.That(JsonArrayObjects.Parse("[    ]"), Is.Empty);
            Assert.That(JsonArrayObjects.Parse("[\n\n]"), Is.Empty);
            Assert.That(JsonArrayObjects.Parse("[\t\t]"), Is.Empty);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_empty_array_with_mixed_whitespaces()
        {
            Assert.That(JsonArrayObjects.Parse("[ \n\t  \n\r]"), Is.Empty);
        }

        public class NamesTest
        {
#if NETCF
            public NamesTest() { }
#endif

            public NamesTest(List<string> names)
            {
                Names = names;
            }

            public List<string> Names { get; set; }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_parse_empty_array_in_dto_with_tab()
        {
            var prettyJson = "{\"Names\":[\t]}";
            var oPretty = prettyJson.FromJson<NamesTest>();
            Assert.That(oPretty.Names.Count, Is.EqualTo(0));
        }
    }
}
