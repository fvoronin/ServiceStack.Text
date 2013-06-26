using System;
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

	#region Test types

	public class GetOnlyWithBacking
	{
		long backing;

		public GetOnlyWithBacking(long i)
		{
			backing = i;
		}

		public long Property
		{
			get { return backing; }
		}
	}
	public class GetSetWithBacking
	{
		long backing;

#if NETCF
	    public GetSetWithBacking() { }
#endif

		public GetSetWithBacking(long i)
		{
			Property = i;
		}

		public long Property
		{
			get { return backing; }
			set { backing = value; }
		}
	}

	#endregion

#if NETCF
    [TestClass]
#endif
    [TestFixture]
	public class BackingFieldTests
	{
#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Backed_get_set_properties_can_be_deserialised()
		{
			var original = new GetSetWithBacking(123344044);
			var str1 = original.ToJson();
			var copy = str1.FromJson<GetSetWithBacking>();

			Console.WriteLine(str1);

			Assert.That(copy.Property, Is.EqualTo(original.Property));
		}

#if !NETCF
        [Ignore("By Design: Deserialization doesn't use constructor injection, Properties need to be writeable")]
		[Test]
		public void Backed_get_properties_can_be_deserialised()
		{
			var original = new GetOnlyWithBacking(123344044);
			var str1 = original.ToJson();
			var copy = str1.FromJson<GetOnlyWithBacking>();

			Console.WriteLine(str1);

			// ReflectionExtensions.cs Line 417 is being used to determine *deserialisable*
			// for properties type based on if the property is *readable*, not *writable* -- by design

            //Rule: To be emitted properties should be readable, to be deserialized properties should be writeable

			Assert.That(copy.Property, Is.EqualTo(original.Property));
		}
#endif
	}
}
