using System;
using NUnit.Framework;
#if !MONOTOUCH
using ServiceStack.ServiceModel.Serialization;
#endif
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
    public class DateTimeOffsetAndTimeSpanTests : TestBase
    {
#if !MONOTOUCH
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            JsonDataContractSerializer.Instance.UseBcl = true;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            JsonDataContractSerializer.Instance.UseBcl = false;            
        }
#endif

#if !NETCF
        [Test]
        public void Can_Serializable_DateTimeOffset_Field()
        {
            var model = new SampleModel { Id = 1, Date = new DateTimeOffset(2012, 6, 27, 11, 26, 04, 524, TimeSpan.FromHours(7)) };

            //Behaviour of .NET's BCL classes
            //JsonDataContractSerializer.Instance.SerializeToString(model).Print();
            //DataContractSerializer.Instance.Parse(model).Print();

            var json = JsonSerializer.SerializeToString(model);
            Assert.That(json, Is.StringContaining("\"TimeSpan\":\"PT0S\""));

            var fromJson = json.FromJson<SampleModel>();

            Assert.That(model.Date, Is.EqualTo(fromJson.Date));
            Assert.That(model.TimeSpan, Is.EqualTo(fromJson.TimeSpan));

            Serialize(fromJson);
        }
#endif

        [Test]
#if NETCF
        [TestMethod]
#endif
        public void Can_serialize_TimeSpan_field()
        {
            var fromDate = new DateTime(2069, 01, 02);
            var toDate = new DateTime(2079, 01, 02);
            var period = toDate - fromDate;

            var model = new SampleModel { Id = 1, TimeSpan = period };
            var json = JsonSerializer.SerializeToString(model);
            Assert.That(json, Is.StringContaining("\"TimeSpan\":\"P3652D\""));

            //Behaviour of .NET's BCL classes
            //JsonDataContractSerializer.Instance.SerializeToString(model).Print();
            //DataContractSerializer.Instance.Parse(model).Print();

            Serialize(model);
        }

        [Test]
        public void Can_serialize_TimeSpan_field_with_StandardTimeSpanFormat()
        {
#if NETCF // TODO NETCF make perfect
            using (JsConfig.With(null, null, null, null, null, null, null, null,
                JsonTimeSpanHandler.StandardFormat,
                null, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(timeSpanHandler:JsonTimeSpanHandler.StandardFormat))
#endif
            {
                var period = TimeSpan.FromSeconds(70);

                var model = new SampleModel { Id = 1, TimeSpan = period };
                var json = JsonSerializer.SerializeToString(model);
                Assert.That(json, Is.StringContaining("\"TimeSpan\":\"00:01:10\""));
            }
        }

        [Test]
        public void Can_serialize_NullableTimeSpan_field_with_StandardTimeSpanFormat()
        {
#if NETCF // TODO NETCF make perfect
            using (JsConfig.With(null, null, null, null, null, null, null, null,
                JsonTimeSpanHandler.StandardFormat,
                null, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(timeSpanHandler:JsonTimeSpanHandler.StandardFormat))
#endif
            {
                var period = TimeSpan.FromSeconds(70);

                var model = new NullableSampleModel { Id = 1, TimeSpan = period };
                var json = JsonSerializer.SerializeToString(model);
                Assert.That(json, Is.StringContaining("\"TimeSpan\":\"00:01:10\""));
            }
        }

        [Test]
        public void Can_serialize_NullTimeSpan_field_with_StandardTimeSpanFormat()
        {
#if NETCF // TODO NETCF make perfect
            using (JsConfig.With(null, null, null, null, null, null, null, null,
                JsonTimeSpanHandler.StandardFormat,
                null, null, null, null, null, null, null, null, null, null, null))
#else
            using (JsConfig.With(timeSpanHandler:JsonTimeSpanHandler.StandardFormat))
#endif
            {
                var model = new NullableSampleModel { Id = 1 };
                var json = JsonSerializer.SerializeToString(model);
                Assert.That(json, Is.Not.StringContaining("\"TimeSpan\""));
            }
        }

        public class SampleModel
        {
            public int Id { get; set; }

#if !NETCF
            public DateTimeOffset Date { get; set; }
#endif
            public TimeSpan TimeSpan { get; set; }
        }

        public class NullableSampleModel
        {
            public int Id { get; set; }

#if !NETCF
            public DateTimeOffset Date { get; set; }
#endif
            public TimeSpan? TimeSpan { get; set; }
        }
    }
}
