using System;
using NUnit.Framework;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using TestCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute;
using ClassCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
#endif

namespace ServiceStack.Text.Tests.JsonTests
{
#if NETCF
    [TestClass]
#endif
    [TestFixture]
    public class CustomRawSerializerTests
    {
#if NETCF
        [ClassCleanup]
#endif
        [TestFixtureTearDown]
        public static void TestFixtureTearDown()
        {
            JsConfig.Reset();
        }
        
        public class RealType
        {
            public string Name { get; set; }
            public byte[] Data { get; set; }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_Serialize_TypeProperties_WithCustomFunction()
        {
            var test = new RealType { Name = "Test", Data = new byte[] { 1, 2, 3, 4, 5 } };

            // Act: now we set a custom function for byte[]
            JsConfig<byte[]>.RawSerializeFn = c =>
                {
                    var temp = new int[c.Length];
                    Array.Copy(c, temp, c.Length);
                    return JsonSerializer.SerializeToString(temp);
                };
            var json = JsonSerializer.SerializeToString(test);

            // Assert:
            Assert.That(json, Is.EquivalentTo("{\"Name\":\"Test\",\"Data\":[1,2,3,4,5]}"));
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_Serialize_AnonymousTypeProperties_WithCustomFunction()
        {
            var test = new { Name = "Test", Data = new byte[] { 1, 2, 3, 4, 5 } };

            // Act: now we set a custom function for byte[]
            JsConfig<byte[]>.RawSerializeFn = c =>
                {
                    var temp = new int[c.Length];
                    Array.Copy(c, temp, c.Length);
                    return JsonSerializer.SerializeToString(temp);
                };
            var json = JsonSerializer.SerializeToString(test);

            // Assert:
            Assert.That(json, Is.EquivalentTo("{\"Name\":\"Test\",\"Data\":[1,2,3,4,5]}"));
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Reset_ShouldClear_JsConfigT_CachedFunctions()
        {
            var test = new { Name = "Test", Data = new byte[] { 1, 2, 3, 4, 5 } };
            JsConfig<byte[]>.RawSerializeFn = c =>
                {
                    var temp = new int[c.Length];
                    Array.Copy(c, temp, c.Length);
                    return JsonSerializer.SerializeToString(temp);
                };
            var json = JsonSerializer.SerializeToString(test);

            Assert.That(json, Is.EquivalentTo("{\"Name\":\"Test\",\"Data\":[1,2,3,4,5]}"));
            // Act: now we set a custom function for byte[]
            JsConfig.Reset();
            json = JsonSerializer.SerializeToString(test);
            // Assert:
            Assert.That(json, Is.EquivalentTo("{\"Name\":\"Test\",\"Data\":\"AQIDBAU=\"}"));
        }        
    }
}