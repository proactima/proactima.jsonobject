using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace proactima.jsonobject.tests
{
    public class DescribeImmutableJsonObjectSerialization
    {
        [Fact]
        public void ItShouldDataContractSerialize_GivenComplexObject()
        {
            // g
            var json = new JsonObject
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

            var complexObject = ImmutableJsonObject.FromMutable(json);

            // w
            var actual = WcfTestHelper.DataContractSerializationRoundTrip(complexObject);

            // t
            actual.GetList<ImmutableJsonObject>("array").Should().HaveCount(2);
            actual.GetValuesFromReference("ref").Should().HaveCount(2);
        }
    }
}
