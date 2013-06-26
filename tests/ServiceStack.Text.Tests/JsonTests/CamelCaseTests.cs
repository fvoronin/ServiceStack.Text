using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using ServiceStack.Text.Tests.Support;
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
#if NETCF
    [TestClass]
#endif
    [TestFixture]
	public class CamelCaseTests : TestBase
	{
#if NETCF
        [TestInitialize]
#endif
        [SetUp]
		public void SetUp()
		{
			JsConfig.EmitCamelCaseNames = true;
		}

#if NETCF
        [TestCleanup]
#endif
        [TearDown]
		public void TearDown()
		{
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Does_serialize_To_CamelCase()
		{
			var dto = new Movie
			{
				Id = 1,
				ImdbId = "tt0111161",
				Title = "The Shawshank Redemption",
				Rating = 9.2m,
				Director = "Frank Darabont",
				ReleaseDate = new DateTime(1995, 2, 17, 0, 0, 0, DateTimeKind.Utc),
				TagLine = "Fear can hold you prisoner. Hope can set you free.",
				Genres = new List<string> { "Crime", "Drama" },
			};

			var json = dto.ToJson();

			Assert.That(json, Is.EqualTo(
				"{\"id\":1,\"imdbId\":\"tt0111161\",\"title\":\"The Shawshank Redemption\",\"rating\":9.2,\"director\":\"Frank Darabont\",\"releaseDate\":\"\\/Date(792979200000)\\/\",\"tagLine\":\"Fear can hold you prisoner. Hope can set you free.\",\"genres\":[\"Crime\",\"Drama\"]}"));

			Serialize(dto);
		}

		[DataContract]
		class Person
		{
			[DataMember(Name = "MyID")]
			public int Id { get; set; }
			[DataMember]
			public string Name { get; set; }
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_override_name()
		{
			var person = new Person
			{
				Id = 123,
				Name = "Abc"
			};

			Assert.That(TypeSerializer.SerializeToString(person), Is.EqualTo("{MyID:123,name:Abc}"));
			Assert.That(JsonSerializer.SerializeToString(person), Is.EqualTo("{\"MyID\":123,\"name\":\"Abc\"}"));
		}

	}
}