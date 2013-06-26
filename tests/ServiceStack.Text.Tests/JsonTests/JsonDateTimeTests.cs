﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
#if !MONOTOUCH && !NETCF
using ServiceStack.Client;
#endif
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
    public class JsonDateTimeTests
	{
	    private string _localTimezoneOffset;

#if NETCF
        [TestInitialize]
#endif
        [SetUp]
        public void SetUp()
        {
            JsConfig.Reset();
#if !NETCF
            _localTimezoneOffset = TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("00") + TimeZoneInfo.Local.BaseUtcOffset.Minutes.ToString("00");
#else
            _localTimezoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString("00") +
                                   TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Minutes.ToString("00");
#endif
        }

		#region TimestampOffset Tests
#if NETCF
        [TestMethod]
#endif
        [Test]
        public void When_using_TimestampOffset_and_serializing_as_Utc_It_should_deserialize_as_Utc()
        {
            JsConfig.DateHandler = JsonDateHandler.TimestampOffset;
            var initialDate = new DateTime(2012, 7, 25, 16, 17, 00, DateTimeKind.Utc);
            var json = JsonSerializer.SerializeToString(initialDate); //"2012-07-25T16:17:00.0000000Z"

            var deserializedDate = JsonSerializer.DeserializeFromString<DateTime>(json);

            Assert.AreEqual(DateTimeKind.Utc, deserializedDate.Kind);
            Assert.AreEqual(initialDate, deserializedDate);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_timestampOffset_utc()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Utc);
			var ssJson = JsonSerializer.SerializeToString(dateTime);

			Assert.That(ssJson, Is.EqualTo(@"""\/Date(785635200000)\/"""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_timestampOffset_local()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Local);
			var ssJson = JsonSerializer.SerializeToString(dateTime);

#if !NETCF
			var offsetSpan = TimeZoneInfo.Local.GetUtcOffset(dateTime);
#else
			var offsetSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
#endif
			var ticks = 785635200000 - offsetSpan.TotalMilliseconds;
			var offset = offsetSpan.ToTimeOffsetString();

			Assert.That(ssJson, Is.EqualTo(@"""\/Date(" + ticks + offset + @")\/"""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_timestampOffset_unspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			// Unspecified time emits '-0000' offset and treated as local time when parsed

			var dateTime1 = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Unspecified);
			var ssJson1 = JsonSerializer.SerializeToString(dateTime1);

            Assert.That(ssJson1, Is.EqualTo(@"""\/Date(785653200000-0000)\/"""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_timestampOffset_withoutOffset_asUtc()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			const string json = @"""\/Date(785635200000)\/""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Utc);
			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_timestampOffset_withOffset_asUnspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			const string json = @"""\/Date(785660400000-0700)\/""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Unspecified);
			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_timestampOffset_withZeroOffset_asUnspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.TimestampOffset;

			const string json = @"""\/Date(785635200000+0000)\/""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Unspecified);
			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

		#endregion

        #region TimeSpan Tests
#if NETCF
        [TestMethod]
#endif
        [Test]
        public void JsonSerializerReturnsTimeSpanAsString()
        {
            Assert.AreEqual("\"PT0S\"", JsonSerializer.SerializeToString(new TimeSpan()));
            Assert.AreEqual("\"PT0.0000001S\"", JsonSerializer.SerializeToString(new TimeSpan(1)));
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void JsonDeserializerReturnsTimeSpanFromString()
        {
            Assert.AreEqual(TimeSpan.Zero, JsonSerializer.DeserializeFromString<TimeSpan>("\"PT0S\""));
            Assert.AreEqual(new TimeSpan(1), JsonSerializer.DeserializeFromString<TimeSpan>("\"PT0.0000001S\""));
        }
        #endregion

        #region DCJS Compatibility Tests
#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_dcjsCompatible_utc()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Utc);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
            var bclJson = @"""\/Date(785635200000)\/"""; //BclJsonDataContractSerializer.Instance.Parse(dateTime);

			Assert.That(ssJson, Is.EqualTo(bclJson));
			JsConfig.Reset();
		}

#if !__MonoCS__ && !NETCF
		[Test]
		public void Can_serialize_json_date_dcjsCompatible_local()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Local);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
            var bclJson = BclJsonDataContractSerializer.Instance.Parse(dateTime);

			Assert.That(ssJson, Is.EqualTo(bclJson));
			JsConfig.Reset();
		}

		[Test]
		public void Can_serialize_json_date_dcjsCompatible_unspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Unspecified);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
            var bclJson = BclJsonDataContractSerializer.Instance.Parse(dateTime);

            Assert.That(ssJson, Is.EqualTo(bclJson));
			JsConfig.Reset();
		}
#endif

#if !MONOTOUCH && !NETCF
		[Test]
		public void Can_deserialize_json_date_dcjsCompatible_utc()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Utc);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(ssJson);
			var fromBclJson = BclJsonDataContractDeserializer.Instance.Parse<DateTime>(ssJson);

			Assert.That(fromJson, Is.EqualTo(fromBclJson));
            Assert.That(fromJson.Kind, Is.EqualTo(DateTimeKind.Utc)); // fromBclJson.Kind
			JsConfig.Reset();
		}

		[Test]
		public void Can_deserialize_json_date_dcjsCompatible_local()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Local);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(ssJson);
			var fromBclJson = BclJsonDataContractDeserializer.Instance.Parse<DateTime>(ssJson);

			Assert.That(fromJson, Is.EqualTo(fromBclJson));
            Assert.That(fromJson.Kind, Is.EqualTo(DateTimeKind.Local)); // fromBclJson.Kind
			JsConfig.Reset();
		}

		[Test]
		public void Can_deserialize_json_date_dcjsCompatible_unspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;

			var dateTime = new DateTime(1994, 11, 24, 0, 0, 0, DateTimeKind.Unspecified);
			var ssJson = JsonSerializer.SerializeToString(dateTime);
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(ssJson);
			var fromBclJson = BclJsonDataContractDeserializer.Instance.Parse<DateTime>(ssJson);

			Assert.That(fromJson, Is.EqualTo(fromBclJson));
            Assert.That(fromJson.Kind, Is.EqualTo(DateTimeKind.Local)); // fromBclJson.Kind
			JsConfig.Reset();
		}
#endif
		#endregion

		#region ISO-8601 Tests
#if NETCF
        [TestMethod]
#endif
        [Test]
        public void When_using_ISO8601_and_serializing_as_Utc_It_should_deserialize_as_Utc()
        {
            JsConfig.AlwaysUseUtc = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            var initialDate = new DateTime(2012, 7, 25, 16, 17, 00, DateTimeKind.Utc);
            var json = JsonSerializer.SerializeToString(initialDate); //"2012-07-25T16:17:00.0000000Z"

            var deserializedDate = JsonSerializer.DeserializeFromString<DateTime>(json);

            Assert.AreEqual(DateTimeKind.Utc, deserializedDate.Kind);
            Assert.AreEqual(initialDate, deserializedDate);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_iso8601_utc()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Utc);
			var ssJson = JsonSerializer.SerializeToString(dateTime);

			Assert.That(ssJson, Is.EqualTo(@"""1994-11-24T12:34:56.0000000Z"""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_iso8601_local()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Local);
			var ssJson = JsonSerializer.SerializeToString(dateTime);

#if !NETCF
			var offsetSpan = TimeZoneInfo.Local.GetUtcOffset(dateTime);
#else
			var offsetSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
#endif
			var offset = offsetSpan.ToTimeOffsetString(":");

			Assert.That(ssJson, Is.EqualTo(@"""1994-11-24T12:34:56.0000000" + offset + @""""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_serialize_json_date_iso8601_unspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Unspecified);
			var ssJson = JsonSerializer.SerializeToString(dateTime);

			Assert.That(ssJson, Is.EqualTo(@"""1994-11-24T12:34:56.0000000"""));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_iso8601_withZOffset_asUtc()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			const string json = @"""1994-11-24T12:34:56Z""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Utc);
			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_iso8601_withoutOffset_asUnspecified()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			const string json = @"""1994-11-24T12:34:56""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Unspecified);
			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

#if NETCF
        [TestMethod]
#endif
        [Test]
		public void Can_deserialize_json_date_iso8601_withOffset_asLocal()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTime = new DateTime(1994, 11, 24, 12, 34, 56, DateTimeKind.Local);
#if !NETCF
			var offset = TimeZoneInfo.Local.GetUtcOffset(dateTime).ToTimeOffsetString(":");
#else
			var offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime).ToTimeOffsetString(":");
#endif

			var json = @"""1994-11-24T12:34:56" + offset + @"""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTime>(json);


			Assert.That(fromJson, Is.EqualTo(dateTime));
			Assert.That(fromJson.Kind, Is.EqualTo(dateTime.Kind));
			JsConfig.Reset();
		}

		#endregion

#if !NETCF
		#region ISO-8601 TimeStampOffset Tests
		[Test]
		public void Can_serialize_json_datetimeoffset_iso8601_utc()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.Zero);
			var ssJson = JsonSerializer.SerializeToString(dateTimeOffset);

			Assert.That(ssJson, Is.EqualTo(@"""1994-11-24T12:34:56.0000000+00:00"""));
			JsConfig.Reset();
		}

		[Test]
		public void Can_serialize_json_datetimeoffset_iso8601_specified()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.FromHours(-7));
			var ssJson = JsonSerializer.SerializeToString(dateTimeOffset);

			Assert.That(ssJson, Is.EqualTo(@"""1994-11-24T12:34:56.0000000-07:00"""));
			JsConfig.Reset();
		}

		[Test]
		public void Can_deserialize_json_datetimeoffset_iso8601_withZOffset_asUtc()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			const string json = @"""1994-11-24T12:34:56Z""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTimeOffset>(json);

			var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.Zero);
			Assert.That(fromJson, Is.EqualTo(dateTimeOffset));
			JsConfig.Reset();
		}

		[Test]
		public void Can_deserialize_json_datetimeoffset_iso8601_withoutOffset_asUtc()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			const string json = @"""1994-11-24T12:34:56""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTimeOffset>(json);

			var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.Zero);
			Assert.That(fromJson, Is.EqualTo(dateTimeOffset));
			JsConfig.Reset();
		}

		[Test]
		public void Can_deserialize_json_datetimeoffset_iso8601_withOffset_asSpecified()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;

			var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.FromHours(-7));

			const string json = @"""1994-11-24T12:34:56-07:00""";
			var fromJson = JsonSerializer.DeserializeFromString<DateTimeOffset>(json);

			Assert.That(fromJson, Is.EqualTo(dateTimeOffset));
			JsConfig.Reset();
		}
		#endregion

        #region InteropTests

        [Test]
        public void Can_serialize_TimestampOffset_deserialize_ISO8601()
        {
            var dateTimeOffset = new DateTimeOffset(1997, 11, 24, 12, 34, 56, TimeSpan.FromHours(-10));

            JsConfig.DateHandler = JsonDateHandler.TimestampOffset;
            var json = ServiceStack.Text.Common.DateTimeSerializer.ToWcfJsonDateTimeOffset(dateTimeOffset);

            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            var fromJson = ServiceStack.Text.Common.DateTimeSerializer.ParseDateTimeOffset(json);

            Assert.That(fromJson, Is.EqualTo(dateTimeOffset));
            JsConfig.Reset();
        }

        [Test]
        public void Can_serialize_ISO8601_deserialize_DCJSCompatible()
        {
            var dateTimeOffset = new DateTimeOffset(1994, 11, 24, 12, 34, 56, TimeSpan.FromHours(-10));

            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            var json = ServiceStack.Text.Common.DateTimeSerializer.ToWcfJsonDateTimeOffset(dateTimeOffset);

            JsConfig.DateHandler = JsonDateHandler.DCJSCompatible;
            var fromJson = ServiceStack.Text.Common.DateTimeSerializer.ParseDateTimeOffset(json);

            // NOTE: DJCS goes to local, so botches offset
            Assert.That(fromJson, Is.EqualTo(dateTimeOffset));
            JsConfig.Reset();
        }

        [Test]
        public void Can_deserialize_null()
        {
            const string json = (string)null;
            var expected = default(DateTimeOffset);
            var fromJson = ServiceStack.Text.Common.DateTimeSerializer.ParseDateTimeOffset(json);
            Assert.That(fromJson, Is.EqualTo(expected));
        }

        #endregion

        public void Test1()
        {
            var tz = TimeZoneInfo.GetSystemTimeZones().ToList().First(t => t.Id == "Afghanistan Standard Time");

            JsConfig.AlwaysUseUtc = true;
            var date = TimeZoneInfo.ConvertTime(new DateTime(2013, 3, 17, 0, 0, 0, DateTimeKind.Utc), tz);
            date.PrintDump();
            date.ToJson().Print();
        }

#endif

#if NETCF
        [TestMethod]
#endif
        [Test]
	    public void ToUnixTimeTests()
	    {
	        var dates = new[]
	            {
			        DateTime.Now,
			        DateTime.UtcNow,
			        new DateTime(1979, 5, 9),
			        new DateTime(1972, 3, 24, 0, 0, 0, DateTimeKind.Local),
			        new DateTime(1972, 4, 24),
			        new DateTime(1979, 5, 9, 0, 0, 1),
			        new DateTime(1979, 5, 9, 0, 0, 0, 1),
			        new DateTime(2010, 10, 20, 10, 10, 10, 1),
			        new DateTime(2010, 11, 22, 11, 11, 11, 1),
                    new DateTime(1970, 1, 1, 1, 1, 1, DateTimeKind.Unspecified),
                    new DateTime(1991, 1, 1, 1, 1, 1, DateTimeKind.Unspecified),
                    new DateTime(2001, 1, 1, 1, 1, 1, DateTimeKind.Unspecified),
                    new DateTime(622119282055250000)
	            }.ToList();

            dates.ForEach(x => "{0} == {1} :: {2}".Print(x.ToUnixTimeMs(), x.ToUnixTimeMsAlt(), x.ToUnixTimeMs() == x.ToUnixTimeMsAlt()));
            Assert.That(dates.All(x => x.ToUnixTimeMs() == x.ToUnixTimeMsAlt()));
	    }

    }
}
