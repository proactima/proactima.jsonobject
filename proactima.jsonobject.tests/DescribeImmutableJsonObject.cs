using FluentAssertions;
using Xunit;

namespace proactima.jsonobject.tests
{
    public class DescribeImmutableJsonObject
    {
        [Fact]
        public void ItShouldReturnValues()
        {
            // g
            var immutable = ImmutableJsonObject.FromMutable(new JsonObject {{"title", "some title"}});

            // w
            var actual = immutable.GetStringValueOrEmpty("title");

            // t
            actual.Should().Be("some title");
        }

        [Fact]
        public void ItShouldReturnNewInstance_WhenSettingValue()
        {
            // g
            var immutable = ImmutableJsonObject.FromMutable(new JsonObject {{"title", "some title"}});

            // w
            var actual = immutable.SetItem("title", "another title");

            // t
            actual.GetStringValueOrEmpty("title").Should().Be("another title");
            immutable.GetStringValueOrEmpty("title").Should().Be("some title");
        }


        [Fact]
        public void ItShouldReturnNewInstance_WhenCreatingReference()
        {
            // g
            var immutable = ImmutableJsonObject.FromMutable(new JsonObject { { "title", "some title" } });

            // w
            var actual = immutable.CreateEntityReferenceField("resp", "user", new[] {"a"});

            // t
            actual.ContainsKey("e_resp_ids").Should().BeTrue();
            actual.GetValuesFromReference("e_resp_ids").Should().HaveCount(1);
            immutable.ContainsKey("e_resp_ids").Should().BeFalse();
        }
    }
}