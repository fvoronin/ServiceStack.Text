using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using System.Runtime.Serialization;
#if NETCF
using Assert = NUnit.Framework.Assert;
using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
using TestCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute;
using ClassInitializeAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute;
using ClassCleanupAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute;
using TestMethodAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;
#endif

namespace ServiceStack.Text.Tests.JsonTests
{
#if NETCF
    [TestClass]
#endif
    public class CustomSerializerTests : TestBase
    {
        static CustomSerializerTests()
        {
            JsConfig.Reset();
            JsConfig<EntityWithValues>.RawSerializeFn = SerializeEntity;
            JsConfig<EntityWithValues>.RawDeserializeFn = DeserializeEntity;
        }

#if NETCF
        [TestInitialize]
        public void TestInitialize()
        {
            JsConfig.Reset();
            JsConfig<EntityWithValues>.RawSerializeFn = SerializeEntity;
            JsConfig<EntityWithValues>.RawDeserializeFn = DeserializeEntity;
        }

        [ClassCleanup]
#endif
        [TestFixtureTearDown]
        public static void TestFixtureTearDown()
        {
            JsConfig.Reset();
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_serialize_Entity()
        {
            var originalEntity = new EntityWithValues { id = 5, Values = new Dictionary<string, string> { { "dog", "bark" }, { "cat", "meow" } } };
            JsonSerializeAndCompare(originalEntity);
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_serialize_arrays_of_entities()
        {
            var originalEntities = new[] { new EntityWithValues { id = 5, Values = new Dictionary<string, string> { { "dog", "bark" } } }, new EntityWithValues { id = 6, Values = new Dictionary<string, string> { { "cat", "meow" } } } };
            JsonSerializeAndCompare(originalEntities);
        }

        public class EntityWithValues
        {
            private Dictionary<string, string> _values;

            public int id { get; set; }

            public Dictionary<string, string> Values
            {
                get { return _values ?? (_values = new Dictionary<string, string>()); }
                set { _values = value; }
            }

            public override int GetHashCode()
            {
                return this.id.GetHashCode() ^ this.Values.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as EntityWithValues);
            }

            public bool Equals(EntityWithValues other)
            {
                return ReferenceEquals(this, other)
                       || (this.id == other.id && DictionaryEquality(Values, other.Values));
            }

            private bool DictionaryEquality(Dictionary<string, string> first, Dictionary<string, string> second)
            {
                return first.Count == second.Count
                       && first.Keys.All(second.ContainsKey)
                       && first.Keys.All(key => first[key] == second[key]);
            }
        }

        private static string SerializeEntity(EntityWithValues entity)
        {
            var dictionary = entity.Values.ToDictionary(pair => pair.Key, pair => pair.Value);
            if (entity.id > 0)
            {
                dictionary["id"] = entity.id.ToString(CultureInfo.InvariantCulture);
            }
            return JsonSerializer.SerializeToString(dictionary);
        }

        private static EntityWithValues DeserializeEntity(string value)
        {
            var dictionary = JsonSerializer.DeserializeFromString<Dictionary<string, string>>(value);
            if (dictionary == null) return null;
            var entity = new EntityWithValues();
            foreach (var pair in dictionary)
            {
                if (pair.Key == "id")
                {
                    if (!string.IsNullOrEmpty(pair.Value))
                    {
                        entity.id = int.Parse(pair.Value);
                    }
                }
                else
                {
                    entity.Values.Add(pair.Key, pair.Value);
                }
            }
            return entity;
        }

        [DataContract]
        private class Test1Base
        {
            public Test1Base(bool itb, bool itbm)
            {
                InTest1Base = itb; InTest1BaseM = itbm;
            }

            [DataMember]
            public bool InTest1BaseM { get; set; }

            public bool InTest1Base { get; set; }
        }

        [DataContract]
        private class Test1 : Test1Base
        {
            public Test1(bool it, bool itm, bool itb, bool itbm)
                : base(itb, itbm)
            {
                InTest1 = it; InTest1M = itm;
            }

            [DataMember]
            public bool InTest1M { get; set; }

            public bool InTest1 { get; set; }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_Serialize_With_Custom_Constructor()
        {
            bool hit = false;
            JsConfig.ModelFactory = type => {
                if (typeof(Test1) == type)
                {
                    hit = true;
                    return () => new Test1(false, false, true, false);
                }
                return null;
            };

            var t1 = new Test1(true, true, true, true);

            var data = JsonSerializer.SerializeToString(t1);

            var t2 = JsonSerializer.DeserializeFromString<Test1>(data);

            Assert.IsTrue(hit);
            Assert.IsTrue(t2.InTest1BaseM);
            Assert.IsTrue(t2.InTest1M);
            Assert.IsTrue(t2.InTest1Base);
            Assert.IsFalse(t2.InTest1);
        }


        public class Dto
        {
            public string Name { get; set; }
        }

        public interface IHasVersion
        {
            int Version { get; set; }
        }

        public class DtoV1 : IHasVersion
        {
            public int Version { get; set; }
            public string Name { get; set; }

            public DtoV1()
            {
                Version = 1;
            }
        }

#if NETCF
        [TestMethod]
#endif
        [Test]
        public void Can_detect_dto_with_no_Version()
        {
#if NETCF
            using (JsConfig.With(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                modelFactory =>
                {
                if (typeof(IHasVersion).IsAssignableFrom(modelFactory))
                {
                    return () => {
                        var obj = (IHasVersion)modelFactory.CreateInstance();
                        obj.Version = 0;
                        return obj;
                    };
                }
                return () => modelFactory.CreateInstance();
            }))
#else
            using (JsConfig.With(modelFactory:type => {
                if (typeof(IHasVersion).IsAssignableFrom(type))
                {
                    return () => {
                        var obj = (IHasVersion)type.CreateInstance();
                        obj.Version = 0;
                        return obj;
                    };
                }
                return () => type.CreateInstance();
            }))
#endif
            {
                var dto = new Dto { Name = "Foo" };
                var fromDto = dto.ToJson().FromJson<DtoV1>();
                Assert.That(fromDto.Version, Is.EqualTo(0));
                Assert.That(fromDto.Name, Is.EqualTo("Foo"));

                var dto1 = new DtoV1 { Name = "Foo 1" };
                var fromDto1 = dto1.ToJson().FromJson<DtoV1>();
                Assert.That(fromDto1.Version, Is.EqualTo(1));
                Assert.That(fromDto1.Name, Is.EqualTo("Foo 1"));
            }
        }
    }
}