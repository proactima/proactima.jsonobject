using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using proactima.jsonobject.common;
using proactima.jsonobject;
using Xunit;

namespace proactima.jsonobject.tests
{
    public class DescribeJsonObject
    {
        [Fact]
        public void ItComplains_WhenCreatingJsonObject_GivenABogusStringAsInput()
        {
            // g
            const string content = "hi there!";

            // w
            Action act = () => JsonObject.Parse(content);

            // t
            act.ShouldThrow<Newtonsoft.Json.JsonReaderException>();
        }

        [Fact]
        public void ItIsRootObject_GivenParentIdZero()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.IsRootObject;

            // t
            actual.Should().Be(true, "object is not root object");
        }

        [Fact]
        public void ItShouldAlwaysHaveParentId()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.ParentId;

            // t
            actual.Should().Be("0", "Does not return parentId");
        }

        [Fact]
        public void ItShouldAlwaysHaveParentType()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.ParentType;

            // t
            actual.Should().Be("", "does not return parenttype");
        }

        [Fact]
        public void ItShouldAlwaysHaveValidParentId()
        {
            // g
            var json = JsonFactory.CreateMiniEntity(parentId: null);

            // w
            var actual = json.ParentId;

            // t
            actual.Should().Be("0", "Does not return valid parentId");
        }

       
        [Fact]
        public void ItShouldCreateChildren_GivenInputWithChildren()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'children': [{'id': 2},{'id': 3}]}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["children"].Should().BeOfType<List<JsonObject>>();
        }

        [Fact]
        public void ItShouldCreateChildren_GivenInputWithChildrenOnMultipleLevels()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'children': [{'id': 2},{'id': 3, 'children': [{'id': 4},{'id': 5}]}]}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            ((List<JsonObject>)actual["children"]).Last()["children"].Should().BeOfType<List<JsonObject>>();
        }

        [Fact]
        public void ItShouldCreateJsonObject_GivenAStringAsInput()
        {
            // g
            const string content = "{'id': 1}";

            // w
            var actual = JsonObject.Parse(content);

            // t
            actual.ContainsKey("id").Should().BeTrue();
        }

        [Fact]
        public void ItShouldCreateString_GivenInputWithEmptyArray()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'unit_ids': []}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["unit_ids"].Should().BeOfType<object[]>();
        }

        [Fact]
        public void ItShouldCreateString_GivenInputWithValueArray()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'unit_ids': ['1', '2', '3']}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.GetList<string>("unit_ids").Should().HaveCount(3);

        }

        [Fact]
        public void ItShouldLowerCaseKeys_WhenAdding()
        {
            // g

            var json = new JsonObject();

            // w
            json.Add("ONE", "oneValue");

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [Fact]
        public void ItShouldLowerCaseKeys_WhenAddingRange()
        {
            // g
            var json = new JsonObject();

            // w
            json.AddRange(new[] { new KeyValuePair<string, object>("ONE", "oneValue") });
            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [Fact]
        public void ItShouldLowerCaseKeys_WhenAddingUsingIndex()
        {
            // g
            var json = new JsonObject();

            // w
            json["ONE"] = "oneValue";

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [Fact]
        public void ItShouldLowerCaseKeys_WhenUsingObjectInit()
        {
            // w
            var json = new JsonObject { { "ONE", "oneValue" } };

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [Fact]
        public void ItShouldMapFromJObject()
        {
            // g
            var json = JObject.Parse("{'id': 1}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("id").Should().BeTrue();
        }

        [Fact]
        public void ItShouldMapFromJObject_GivenArticleReferences()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("ArticleWithArticleReference.json"));

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            JsonObjectVerificationHelper.VerifyJsonObject(actual, "a_risk_ids", "risk");
        }

        [Fact]
        public void ItShouldMapFromJObject_GivenNullValues()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("JsonWithNull.json"));

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["disabled"].Should().BeNull();

        }

        [Fact]
        public void ItShouldMapFromJObject_GivenEntityReferences()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("ArticleWithEntityReference.json"));

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            JsonObjectVerificationHelper.VerifyJsonObject(actual, "e_assessment_location_ids", "location");
        }

        [Fact]
        public void ItShouldNotRemoveGeneratedReference_GivenYouWantToKeepThem()
        {
            // g
            var json = JObject.Parse("{'id': 1,'a_gen_a':'toberemoved' ,'e_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json, true);

            // t
            actual.Keys.Any(k => k.StartsWith(Constants.GeneratedArticlePrefix)).Should().BeTrue();
            actual.Keys.Any(k => k.StartsWith(Constants.GeneratedEntityPrefix)).Should().BeTrue();
        }        
        
        [Fact]
        public void ItShouldNotRemoveGeneratedReference()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("JsonWithGeneratedStuff.json"));

            // w
            var actual = JsonObject.FromJObject(json, true);

            // t
            actual.GetList<JsonObject>("workshops").First().Should().ContainKey("a_gen_invites_ids");

        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenBooleanInput()
        {
            // g
            var json = JObject.Parse("{'id': true}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].Should().BeOfType<Boolean>();
        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenDateTimeInput()
        {
            // g
            var json = JObject.Parse("{'id': '1'}");
            json.Add("date_deadline", DateTime.UtcNow);

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["date_deadline"].Should().BeOfType<DateTime>();
        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenDecimalInput()
        {
            // g
            var json = JObject.Parse("{'id': '1'}");
            json.Add("decimal", 12.01);

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["decimal"].Should().BeOfType<decimal>();
        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenIntegerInput()
        {
            // g
            var json = JObject.Parse("{'id': 1}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].Should().BeOfType<long>();
        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenObjectInput()
        {
            // g
            var json = JObject.Parse("{'id': {'name': 'someName', 'url':'http...'}}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].Should().BeOfType<JsonObject>();

        }

        [Fact]
        public void ItShouldPreserveDatatype_GivenStringInput()
        {
            // g
            var json = JObject.Parse("{'id': 'true'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].Should().BeOfType<string>();

        }

        [Fact]
        public void ItShouldRemoveGeneratedArticleReference()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'a_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("a_gen_a").Should().BeFalse();
        }

        [Fact]
        public void ItShouldRemoveGeneratedEntityReference()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'e_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("e_gen_a").Should().BeFalse();
        }

        [Fact]
        public void ItShouldRemoveGeneratedReference()
        {
            // g
            var json = JObject.Parse("{'id': 1,'a_gen_a':'toberemoved' ,'e_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.Keys.Any(k => k.StartsWith(Constants.GeneratedArticlePrefix)).Should().BeFalse();
            actual.Keys.Any(k => k.StartsWith(Constants.GeneratedEntityPrefix)).Should().BeFalse();
        }

        [Fact]
        public void ItShouldMapFromJObject_GivenNullProperty()
        {
            // g
            var json = JObject.Parse("{'action': ''}");
            json["action"] = null;

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.Keys.Count.Should().Be(1);
            actual["action"].Should().Be(null);
        }

        [Fact]
        public void ItShouldRetainParentId()
        {
            // g
            var json = JsonFactory.CreateMiniEntity();
            var expected = json.ParentId;

            // w
            var actual = json.ParentId;

            // t
            actual.Should().Be(expected, "Does not retain parentId");
        }

        [Fact]
        public void ItShouldSerialize()
        {
            // g
            var json =
                JsonFactory.CreateMiniEntity();
            var formatter = new BinaryFormatter();
            var ms = new MemoryStream();

            // w
            formatter.Serialize(ms, json);

            // t
            ms.Position = 0;
            var deserialized = (JsonObject)formatter.Deserialize(ms);

            deserialized.Id.Should().NotBeEmpty();
            deserialized.ShouldBeEquivalentTo(json);
        }

        [Fact]
        public void ItShouldVerifyKeyNotPresent()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var hasKey = json.ContainsKey("something that does not exist");

            // t
            hasKey.Should().BeFalse("Does not contain the key!");
        }

        [Fact]
        public void ItShouldVerifyKeyPresent()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var hasKey = json.ContainsKey("name");

            // t
            hasKey.Should().BeTrue("Does not contain the key!");
        }
    }
}