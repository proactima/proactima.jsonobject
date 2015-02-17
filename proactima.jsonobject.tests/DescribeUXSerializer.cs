using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace proactima.jsonobject.tests
{
    [TestClass]
    public class DescribeUXSerializer
    {
        [TestMethod]
        public void ItShouldSerializeDt()
        {
            // g
            var json = DateTime.UtcNow;

            // w
            var ms = UXSerializer.Serialize(json);

            // t
            var deserialized = UXSerializer.Deserialize<DateTime>(ms);
            var jsonAsString = JsonConvert.SerializeObject(json);
            var deserializedAsString = JsonConvert.SerializeObject(deserialized);
            jsonAsString.Should().Be(deserializedAsString);
        }

        [TestMethod]
        public void ItShouldSerialize()
        {
            // g
            var json = JsonFactory.CreateComplexJsonObject();

            // w
            var ms = UXSerializer.Serialize(json);

            // t
            var deserialized = UXSerializer.Deserialize<JsonObject>(ms);
            var jsonAsString = JsonConvert.SerializeObject(json);
            var deserializedAsString = JsonConvert.SerializeObject(deserialized);
            jsonAsString.Should().Be(deserializedAsString);
        }
    }
}