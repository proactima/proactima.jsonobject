using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polenter.Serialization;

namespace proactima.jsonobject.tests
{
    [TestClass]
    public class DescribeJsonObject
    {
        [TestMethod]
        public void ItCanInstantiate()
        {
            // g
            // w
            var json = new JsonObject();

            // t
            Assert.AreSame(json.Id, "0");
        }

        [TestMethod]
        public void ItCanPopulate()
        {
            // g
            // w
            var json = new JsonObject
            {
                {"key", "value"}
            };

            // t
            Assert.AreSame(json.GetStringValueOrEmpty("key"), "value");
        }

        [TestMethod]
        public void ItComplains_WhenCreatingJsonObject_GivenABogusStringAsInput()
        {
            // g
            const string content = "hi there!";

            // w
            Action act = () => JsonObject.Parse(content);

            // t
            act.ShouldThrow<JsonReaderException>();
        }

        [TestMethod]
        public void ItIsRootObject_GivenParentIdZero()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.IsRootObject;

            // t
            actual.Should().Be(true, "object is not root object");
        }

        [TestMethod]
        public void ItShouldAlwaysHaveParentId()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.ParentId;

            // t
            actual.Should().Be("0", "Does not return parentId");
        }

        [TestMethod]
        public void ItShouldAlwaysHaveParentType()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var actual = json.ParentType;

            // t
            actual.Should().Be("", "does not return parenttype");
        }

        [TestMethod]
        public void ItShouldAlwaysHaveValidParentId()
        {
            // g
            var json = JsonFactory.CreateMiniEntity(parentId: null);

            // w
            var actual = json.ParentId;

            // t
            actual.Should().Be("0", "Does not return valid parentId");
        }

        [TestMethod]
        public void ItShouldChangeValueOnElement_GivenValueIsArray()
        {
            // g
            const string idstring = "[\"9\", \"8\"]";
            const string fieldName = "entity_ids";

            // w
            var json = new JsonObject {{fieldName, JArray.Parse(idstring)}};

            // t
            var expectation = JArray.Parse(idstring).Select(t => t.ToObject<object>());
            json.GetList<object>(fieldName).ShouldBeEquivalentTo(expectation);
        }


        [TestMethod]
        public void ItShouldCreateChildren_GivenInputWithChildren()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'children': [{'id': 2},{'id': 3}]}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["children"].GetType().Should().Be(typeof (List<JsonObject>));
        }

        [TestMethod]
        public void ItShouldCreateChildren_GivenInputWithChildrenOnMultipleLevels()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'children': [{'id': 2},{'id': 3, 'children': [{'id': 4},{'id': 5}]}]}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            ((List<JsonObject>) actual["children"]).Last()["children"].GetType().Should().Be(typeof (List<JsonObject>));
        }

        [TestMethod]
        public void ItShouldCreateJsonObject_GivenAStringAsInput()
        {
            // g
            const string content = "{'id': 1}";

            // w
            var actual = JsonObject.Parse(content);

            // t
            actual.ContainsKey("id").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldCreateString_GivenInputWithEmptyArray()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'unit_ids': []}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["unit_ids"].GetType().Should().Be(typeof (object[]));
        }

        [TestMethod]
        public void ItShouldCreateString_GivenInputWithValueArray()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'unit_ids': ['1', '2', '3']}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["unit_ids"].GetType().Should().Be(typeof (object[]));
        }


        [TestMethod]
        public void ItShouldLowerCaseKeys_WhenAdding()
        {
            // g

            var json = new JsonObject();

            // w
            json.Add("ONE", "oneValue");

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldLowerCaseKeys_WhenAddingRange()
        {
            // g
            var json = new JsonObject();

            // w
            json.AddRange(new[] {new KeyValuePair<string, object>("ONE", "oneValue")});
            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldLowerCaseKeys_WhenAddingUsingIndex()
        {
            // g
            var json = new JsonObject();

            // w
            json["ONE"] = "oneValue";

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldLowerCaseKeys_WhenUsingObjectInit()
        {
            // w
            var json = new JsonObject {{"ONE", "oneValue"}};

            // t
            json.ContainsKey("one").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldMapFromJObject()
        {
            // g
            var json = JObject.Parse("{'id': 1}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("id").Should().BeTrue();
        }

        [TestMethod]
        public void ItShouldMapFromJObject_GivenArticleReferences()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("ArticleWithArticleReference.json"));

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            JsonObjectVerificationHelper.VerifyJsonObject(actual, "a_risk_ids", "risk");
        }

        [TestMethod]
        public void ItShouldMapFromJObject_GivenEntityReferences()
        {
            // g
            var json = JObject.Parse(File.ReadAllText("ArticleWithEntityReference.json"));

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            JsonObjectVerificationHelper.VerifyJsonObject(actual, "e_assessment_location_ids", "location");
        }

        [TestMethod]
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

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenBooleanInput()
        {
            // g
            var json = JObject.Parse("{'id': true}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].GetType().Should().Be(typeof (Boolean));
        }

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenDateTimeInput()
        {
            // g
            var json = JObject.Parse("{'id': '1'}");
            json.Add("date_deadline", DateTime.UtcNow);

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["date_deadline"].GetType().Should().Be(typeof (DateTime));
        }

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenDecimalInput()
        {
            // g
            var json = JObject.Parse("{'id': '1'}");
            json.Add("decimal", 12.01);

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["decimal"].GetType().Should().Be(typeof (decimal));
        }

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenIntegerInput()
        {
            // g
            var json = JObject.Parse("{'id': 1}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].GetType().Should().Be(typeof (long));
        }

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenObjectInput()
        {
            // g
            var json = JObject.Parse("{'id': {'name': 'someName', 'url':'http...'}}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].GetType().Should().Be(typeof (JsonObject));
        }

        [TestMethod]
        public void ItShouldPreserveDatatype_GivenStringInput()
        {
            // g
            var json = JObject.Parse("{'id': 'true'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual["id"].GetType().Should().Be(typeof (string));
        }

        [TestMethod]
        public void ItShouldRemoveGeneratedArticleReference()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'a_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("a_gen_a").Should().BeFalse();
        }

        [TestMethod]
        public void ItShouldRemoveGeneratedEntityReference()
        {
            // g
            var json = JObject.Parse("{'id': 1, 'e_gen_a':'toberemoved'}");

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.ContainsKey("e_gen_a").Should().BeFalse();
        }

        [TestMethod]
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

        [TestMethod]
        public void ItShouldMapFromJObject_GivenNullProperty()
        {
            // g
            var json = JObject.Parse("{'action': ''}");
            json["action"] = null;

            // w
            var actual = JsonObject.FromJObject(json);

            // t
            actual.Keys.Count.Should().Be(1);
            actual["action"].Should().Be(string.Empty);
        }

        [TestMethod]
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

        [TestMethod]
        public void ItShouldVerifyKeyNotPresent()
        {
            // g
            var json = JsonFactory.CreateObjectWithoutId();

            // w
            var hasKey = json.ContainsKey("something that does not exist");

            // t
            hasKey.Should().BeFalse("Does not contain the key!");
        }

        [TestMethod]
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