using FluentAssertions;

namespace proactima.jsonobject.tests
{
    public static class JsonObjectVerificationHelper
    {
        public static void VerifyJsonObject(JsonObject actual, string aEventIds, string typeName)
        {
            actual.ContainsKey("id").Should().BeTrue();
            actual.ContainsKey(aEventIds).Should().BeTrue();
            actual[aEventIds].GetType().Should().Be(typeof (JsonObject));
            ((JsonObject) actual[aEventIds])["type"].Should().Be(typeName);
            ((JsonObject) actual[aEventIds])["values"].GetType().Should().Be(typeof (object[]));
        }
    }
}