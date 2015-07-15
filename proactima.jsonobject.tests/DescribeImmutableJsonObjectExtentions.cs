using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace proactima.jsonobject.tests
{
	public class DescribeImmutableJsonObjectExtentions
	{
		[Fact]
		public void ItCanConvertSimpleImmutableToMutable()
		{
			var immutable = ImmutableJsonObject.FromMutable(new JsonObject { { "title", "some title" } });

			var actual = immutable.ToMutable();
			actual.GetType().Should().Be<JsonObject>();
		}

		[Fact]
		public void ItCanConvertComplexImmutableToMutable()
		{
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

			var immutable = ImmutableJsonObject.FromMutable(json);

			var actual = immutable.ToMutable();

			actual.GetType().Should().Be<JsonObject>();
			actual["array"].GetType().Should().Be<List<JsonObject>>();
		}
	}
}