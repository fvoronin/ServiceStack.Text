using System.Collections;
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
    public class IDictionarySerializationTests
    {
#if NETCF
        [TestMethod]
#endif
        [Test]
        public void CanSerializeHashtable()
        {
            var hash = new Hashtable();

            hash["a"] = "b";
            hash[1] = 1;
            hash[2.0m] = 2.0m;

            var serialized = JsonSerializer.SerializeToString(hash);

            var deserialized = JsonSerializer.DeserializeFromString<Dictionary<string, object>>(serialized);

            Assert.AreEqual("b", deserialized["a"]);
            Assert.AreEqual("1", deserialized["1"]);
            Assert.AreEqual("2.0", deserialized["2.0"]);
        }
    }
}