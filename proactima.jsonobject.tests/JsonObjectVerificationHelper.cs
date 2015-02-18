using System;
using FluentAssertions;

namespace proactima.jsonobject.tests
{
    public static class JsonObjectVerificationHelper
    {
        public static void VerifyJsonObject(JsonObject actual, string aEventIds, string typeName)
        {
            actual.ContainsKey("id").Should().BeTrue();
            actual.ContainsKey(aEventIds).Should().BeTrue();
            actual[aEventIds].GetType().Should().Be(typeof(JsonObject));
            ((JsonObject)actual[aEventIds])["type"].Should().Be(typeName);
            ((JsonObject)actual[aEventIds])["values"].GetType().Should().Be(typeof(object[]));
        }

        public static void VerifyKeys(this JsonObject actual, Func<JsonObject> json, string key)
        {
            var obj = json();
            actual[key].ShouldBeEquivalentTo(obj[key]);
        }

        public static void Verify(this JsonObject[] json, JsonObject[] inner, int numberOfObjects = -1)
        {
            json.Should().BeOfType(typeof(JsonObject[]));
            if (inner != null)
                json.ShouldBeEquivalentTo(inner);
            if (numberOfObjects >= 0)
                json.Should().HaveCount(numberOfObjects);
        }
    }
}