using FluentAssertions;
using Xunit;
using proactima.jsonobject;
namespace proactima.jsonobject.tests
{
    public class DescribeSerialization
    {
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