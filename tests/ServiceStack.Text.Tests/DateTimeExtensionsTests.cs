using System;
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
    public class DateTimeExtensionsTests
    {
        [TestCase]
#if NETCF
        [TestMethod]
#endif
        public void LastMondayTest()
        {
            var monday = new DateTime(2013, 04, 15);

            var lastMonday = DateTimeExtensions.LastMonday(monday);

            Assert.AreEqual(monday, lastMonday);
        } 
    }
}