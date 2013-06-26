using System;
using NUnit.Framework;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using TestCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
#endif

namespace ServiceStack.Text.Tests.JsonTests
{
    /// <summary>
    /// Service Bus messaging works best if processes can share interface message contracts
    /// but not have to share concrete types.
    /// </summary>
#if NETCF
    [TestClass]
#endif
    [TestFixture]
    public class ContractByInterfaceTests
    {
#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Prefer_interfaces_should_work_on_top_level_object_using_extension_method()
        {
#if NETCF
            using (JsConfig.With(null, null, null, null, null, null, null, null, null, /*preferInterfaces*/true, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(preferInterfaces:true))
#endif
            {
                var json = new Concrete("boo", 1).ToJson();

                Assert.That(json, Is.StringContaining("\"ServiceStack.Text.Tests.JsonTests.IContract, ServiceStack.Text.Tests\""));
            }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Should_be_able_to_serialise_based_on_an_interface()
        {
#if NETCF
            using (JsConfig.With(null, null, null, null, null, null, null, null, null, /*preferInterfaces*/true, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(preferInterfaces:true))
#endif
            {
                IContract myConcrete = new Concrete("boo", 1);
                var json = JsonSerializer.SerializeToString(myConcrete, typeof(IContract));

                Console.WriteLine(json);
                Assert.That(json, Is.StringContaining("\"ServiceStack.Text.Tests.JsonTests.IContract, ServiceStack.Text.Tests\""));
            }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Should_not_use_interface_type_if_concrete_specified()
        {
#if NETCF
            using (JsConfig.With(null, null, null, null, null, null, null, null, null, /*preferInterfaces*/false, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(preferInterfaces:false))
#endif
            {
                IContract myConcrete = new Concrete("boo", 1);
                var json = JsonSerializer.SerializeToString(myConcrete, typeof(IContract));

                Console.WriteLine(json);
                Assert.That(json, Is.StringContaining("\"ServiceStack.Text.Tests.JsonTests.Concrete, ServiceStack.Text.Tests\""));
            }
        }

#if NETCF
        [TestMethod]
        [Ignore] // Deserialization from Interface and Abstract type does not supported
#endif
        [Test]
        public void Should_be_able_to_deserialise_based_on_an_interface_with_no_concrete()
        {
#if NETCF
            using (JsConfig.With(null, null, null, null, null, null, null, null, null, /*preferInterfaces*/true, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(preferInterfaces:true))
#endif
            {
                var json = new Concrete("boo", 42).ToJson();

                // break the typing so we have to use the dynamic implementation
                json = json.Replace("ServiceStack.Text.Tests.JsonTests.IContract", "ServiceStack.Text.Tests.JsonTests.IIdenticalContract");

                var result = JsonSerializer.DeserializeFromString<IIdenticalContract>(json);

                Assert.That(result.StringValue, Is.EqualTo("boo"));
                Assert.That(result.ChildProp.IntValue, Is.EqualTo(42));
            }
        }
    }

    class Concrete : IContract
    {
        public Concrete(string boo, int i)
        {
            StringValue = boo;
            ChildProp = new ConcreteChild { IntValue = i };
        }

        public string StringValue { get; set; }
        public IChildInterface ChildProp { get; set; }
    }
    class ConcreteChild : IChildInterface
    {
        public int IntValue { get; set; }
    }

    public interface IChildInterface
    {
        int IntValue { get; set; }
    }
    public interface IContract
    {
        string StringValue { get; set; }
        IChildInterface ChildProp { get; set; }
    }
    public interface IIdenticalContract
    {
        string StringValue { get; set; }
        IChildInterface ChildProp { get; set; }
    }
}
