using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using FluentAssertions;
using Xunit;

namespace proactima.jsonobject.tests
{
    [DataContract]
    public class Boo
    {
        [DataMember]
        public ImmutableJsonObject Json { get; set; }
    }

    public class DescribeJsonObjectSerialization
    {
        [Fact]
        public void ItShouldSerializeAsBalls()
        {
            var json = new JsonObject
            {
                {"test", "1 < 2 > 1"}
            };
            var t = new Boo
            {
                Json = ImmutableJsonObject.FromMutable(json)
            };
            var actual = WcfTestHelper.DataContractSerializationRoundTrip(t);
            actual.Json.GetStringValueOrEmpty("test").Should().Be("1 < 2 > 1");
        }


        [Fact]
        public void ItShouldSerializeAsBase64EncodedString()
        {


            var json = new JsonObject
            {
                {"test", "1 < 2 > 1"}
            };
            var x = new XmlSerializer(typeof (JsonObject));

            var ms = new MemoryStream();
            var txtW = new StreamWriter(ms);
            x.Serialize(txtW, json);
            ms.Position = 0;
            var de = (JsonObject) x.Deserialize(ms);
            de["test"].Should().Be("1 < 2 > 1");
        }


       

        [Fact]
        public void ItShouldDeserializeAsRawString()
        {
            var x = new XmlSerializer(typeof (JsonObject));

            var xml = new XmlDocument();
            var dec = xml.CreateXmlDeclaration("1.0", "utf - 16", null);

            var el = xml.CreateElement("JsonObject");
            el.InnerText = "{ \"test\":\"some random string\"}";

            xml.AppendChild(dec);
            xml.AppendChild(el);

            var asString = xml.OuterXml;
            var txtR = new StringReader(asString);
            var de = (JsonObject) x.Deserialize(txtR);

            de["test"].Should().Be("some random string");
        }


        [Fact]
        public void ItShouldDataContractSerialize_GivenComplexObject()
        {
            // g
            var complexObject = new JsonObject
            {
                {"title", "the title"},
                {"array", new[] {new JsonObject {{"title", "sub1"}}, new JsonObject {{"title", "sub2"}}}},
                {
                    "ref", new JsonObject
                    {
                        {"type", "sometype"},
                        {"values", new[] {"1", "2"}}
                    }
                },
            };

            // w
            var actual = WcfTestHelper.DataContractSerializationRoundTrip(complexObject);

            // t
            actual.GetList<JsonObject>("array").Should().HaveCount(2);
            actual.GetValuesFromReference("ref").Should().HaveCount(2);
        }
    }
}
