using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace proactima.jsonobject.tests
{
    public class DescribeJsonObjectsExtensions
    {
        private void VerifyGetValueOrDefault<T>(T value, Func<JsonObject, string, T> getter) where T : struct
        {
            //g
            const string key = "somekey";
            var jsonObj = new JsonObject {{key, value}};

            //w
            var actual = getter(jsonObj, key);

            //t
            actual.ShouldBeEquivalentTo(value);
        }

        private void VerifyGetValueOrDefaultWhenKeyIsMissing<T>(Func<JsonObject, string, T> getter) where T : struct
        {
            //g
            const string key = "somekey";
            var expected = default(T);
            var jsonObj = new JsonObject();

            //w
            var actual = getter(jsonObj, key);

            //t
            actual.ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData((long) 1, 1)]
        [InlineData("1", 1)]
        [InlineData("asd", 0)]
        [InlineData(false, 0)]
        public void ItCanGetANumber(object value, long expected)
        {
            //g
            const string key = "somekey";
            var jsonObj = new JsonObject {{key, value}};

            //w
            var actual = jsonObj.GetNumberOrDefault(key);

            //t
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanCheckFirstResultETagMatches()
        {
            //g
            var jsonObj = new JsonObject
            {
                {
                    "results", new List<JsonObject>
                    {new JsonObject {{"etag", "123"}}}
                }
            };

            //w
            var actual = jsonObj.FirstResultETagMatches("123");

            //t
            actual.Should().BeTrue();
        }

        [Fact]
        public void ItCanCheckIfETagMatches()
        {
            //g
            var jsonObj = new JsonObject {{"etag", "123"}};

            //w
            var actual = jsonObj.ETagMatches("123");

            //t
            actual.Should().BeTrue();
        }

        [Fact]
        public void ItCanCreateArticleReferenceFieldWithMultipleValues()
        {
            //g
            var jsonObj = new JsonObject();
            var values = new[] {"1", "2"};

            var expected = new JsonObject
            {
                {
                    "a_technicalrisk_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", values}
                    }
                }
            };

            //w
            jsonObj.CreateArticleReferenceField("technicalrisk", "risk", values);

            //t
            jsonObj.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanCreateArticleReferenceFieldWithSingleValue()
        {
            //g
            var jsonObj = new JsonObject();
            const string value = "1";

            var expected = new JsonObject
            {
                {
                    "a_technicalrisk_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new[] {value}}
                    }
                }
            };

            //w
            jsonObj.CreateArticleReferenceField("technicalrisk", "risk", value);

            //t
            jsonObj.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanCreateEntityReferenceFieldWithMultipleValues()
        {
            //g
            var jsonObj = new JsonObject();
            var values = new[] {"1", "2"};

            var expected = new JsonObject
            {
                {
                    "e_customerunit_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", values}
                    }
                }
            };

            //w
            jsonObj.CreateEntityReferenceField("customerunit", "risk", values);

            //t
            jsonObj.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanCreateEntityReferenceFieldWithSingleValue()
        {
            //g
            var jsonObj = new JsonObject();
            const string value = "1";

            var expected = new JsonObject
            {
                {
                    "e_customerunit_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new[] {value}}
                    }
                }
            };

            //w
            jsonObj.CreateEntityReferenceField("customerunit", "risk", value);

            //t
            jsonObj.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanGetAListOfArticleReferenceFields()
        {
            // g
            var json = JsonFactory.CreateObjectWithChildren();

            // w
            var actual = json.GetArticleReferences();

            // t
            actual.Count().Should().Be(1);
        }

        [Fact]
        public void ItCanGetBoolValueOrFalse()
        {
            const bool value = true;

            VerifyGetValueOrDefault(value, JsonObjectExtensions.GetBoolOrFalse);
        }

        [Fact]
        public void ItCanGetDateTimeValueOrDefault()
        {
            var value = new DateTime(2014, 6, 23);

            VerifyGetValueOrDefault(value, JsonObjectExtensions.GetDateTimeOrDefault);
        }

        [Fact]
        public void ItCanGetFirstValueOrDefaultFromReference()
        {
            //g
            const string referenceFieldName = "a_risk_ids";
            const string firstValue = "1";
            var referenceValues = new[] {firstValue, "2"};

            var json = new JsonObject
            {
                {
                    referenceFieldName, new JsonObject
                    {
                        {"type", "risk"},
                        {"values", referenceValues}
                    }
                }
            };

            //w
            var actual = json.GetFirstValueOrEmptyFromReference(referenceFieldName);

            //t
            actual.Should().Be(firstValue);
        }

        [Fact]
        public void ItCanGetIntValueOr0()
        {
            const int value = 1;

            VerifyGetValueOrDefault(value, JsonObjectExtensions.GetNumberOrDefault);
        }

        [Fact]
        public void ItCanGetKey()
        {
            //g
            const string key = "somekey";
            const string value = "someValue";
            var jsonObj = new JsonObject {{key, value}};

            //w
            var actual = jsonObj.GetValue(key);

            //t
            actual.Should().Be(value);
        }

        [Fact]
        public void ItCanGetListWhenKeyIsPresent()
        {
            //g
            var list = new List<JsonObject>
            {
                new JsonObject {{"title", "risk 1"}},
                new JsonObject {{"title", "risk 2"}}
            };

            var jsonObj = new JsonObject
            {
                {"risks", list}
            };

            //w
            var result = jsonObj.GetList<JsonObject>("risks");

            //t
            result.Count.Should().Be(list.Count);
        }

        [Fact]
        public void ItCanGetLongValueOr0()
        {
            const long value = 1;

            VerifyGetValueOrDefault(value, JsonObjectExtensions.GetNumberOrDefault);
        }

        [Fact]
        public void ItCanGetStringValueOrEmpty()
        {
            //g
            const string key = "somekey";
            const string value = "someValue";
            var jsonObj = new JsonObject {{key, value}};

            //w
            var actual = jsonObj.GetStringValueOrEmpty(key);

            //t
            actual.Should().Be(value);
        }

        [Fact]
        public void ItCanGetTypeFromReference()
        {
            //g
            const string referenceFieldName = "a_risk_ids";
            var referenceValues = new object[] {"1", "2"};

            var json = new JsonObject
            {
                {
                    referenceFieldName, new JsonObject
                    {
                        {"type", "risk"},
                        {"values", referenceValues}
                    }
                }
            };

            //w
            var actual = json.GetTypeFromReference(referenceFieldName);

            //t
            actual.Should().Be("risk");
        }

        [Fact]
        public void ItCanGetTypeFromReferenceGivenInnerObjectIsJobject()
        {
            //g
            const string referenceFieldName = "a_risk_ids";

            var json = new JsonObject
            {
                {
                    referenceFieldName, new JObject
                    {
                        {"type", "risk"},
                        {"values", string.Empty}
                    }
                }
            };

            //w
            var actual = json.GetTypeFromReference(referenceFieldName);

            //t
            actual.Should().Be("risk");
        }


        [Fact]
        public void ItCanGetValuesFromReference()
        {
            //g
            const string referenceFieldName = "a_risk_ids";
            var referenceValues = new [] {"1", "2"};

            var json = new JsonObject
            {
                {
                    referenceFieldName, new JsonObject
                    {
                        {"type", "risk"},
                        {"values", referenceValues}
                    }
                }
            };

            //w
            var actual = json.GetValuesFromReference(referenceFieldName);

            //t
            actual.Should().BeEquivalentTo(referenceValues);
        }

        [Fact]
        public void ItCanRetrieveTheReferenceFields()
        {
            //g
            var unicorn = new JsonObject
            {
                {"type", "unicorn"},
                {"values", new[] {"123", "345"}}
            };

            var lion = new JsonObject
            {
                {"type", "lion"},
                {"values", new[] {"abc", "def"}}
            };

            var jsonObj = new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {Constants.Parentid, "123"},
                {"a_lions_ids", lion},
                {"e_unicorns_ids", unicorn},
            };

            var expected = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("a_lions_ids", lion),
                new KeyValuePair<string, object>("e_unicorns_ids", unicorn)
            };

            //w
            var act = jsonObj.GetReferenceFields();

            //t
            act.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItCanReturnResultsAsArray()
        {
            // g
            var inner = new JsonObject {{"name", "somename"}};
            var outer = new JsonObject {{"results", inner}};

            // w
            var expected = outer.Results();

            // t
            expected.Verify(null, 1);
        }

        [Fact]
        public void ItCanReturnResultsAsArrayWithArrayObject()
        {
            // g
            var inner = new[] {new JsonObject {{"name", "somename"}}, new JsonObject {{"name", "somename"}}};
            var outer = new JsonObject {{"results", inner}};

            // w
            var expected = outer.Results();

            // t
            expected.Verify(inner, 2);
        }

        [Fact]
        public void ItCanReturnResultsAsArrayWithListOfObjects()
        {
            // g
            var inner = new List<JsonObject>
            {
                new JsonObject {{"name", "somename"}},
                new JsonObject {{"name", "somename"}}
            };
            var outer = new JsonObject {{"results", inner}};

            // w
            var expected = outer.Results();

            // t
            expected.Verify(inner.ToArray(), 2);
        }

        [Fact]
        public void ItCanReturnResultsAsArrayWithSingleObject()
        {
            // g
            var inner = new JsonObject {{"name", "somename"}};
            var outer = new JsonObject {{"results", inner}};

            // w
            var expected = outer.Results();

            // t
            expected.Verify(new[] {inner}, 1);
        }

        [Fact]
        public void ItCanSetSystemPropertiesFrom()
        {
            //g
            const int expValue = 123;
            var jsonObj = new JsonObject();
            var jsonParent = new JsonObject {{"sys_somefield", expValue}};

            //w
            jsonObj.SetSystemPropertiesFrom(jsonParent);

            //t
            jsonObj.ContainsKey("sys_somefield").Should().BeTrue();
            jsonObj["sys_somefield"].Should().Be(expValue);
        }

        [Fact]
        public void ItCanVerifyThePresenceOfReferenceFields()
        {
            //g
            var jsonObj = new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {Constants.Parentid, "123"},
                {"lions_ids", new[] {"123", "345"}}
            };

            //w
            var act = jsonObj.ContainsReferenceFields();

            //t
            act.Should().BeTrue();
        }

        [Fact]
        public void ItComplainsIfInvalidKey()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            Action act = () => jsonObj.GetValue(null);

            //t
            act.ShouldThrow<ArgumentNullException>()
                .Where(x => x.ParamName == "key");
        }

        [Fact]
        public void ItComplainsIfKeyIsMissing()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            Action act = () => jsonObj.GetList<JsonObject>("");

            //t
            act.ShouldThrow<ArgumentNullException>()
                .Where(x => x.ParamName == "key");
        }

        [Fact]
        public void ItReturns0WhenGettingIntAndKeyIsMissing()
        {
            VerifyGetValueOrDefaultWhenKeyIsMissing(JsonObjectExtensions.GetNumberOrDefault);
        }

        [Fact]
        public void ItReturns0WhenGettingLongAndKeyIsMissing()
        {
            VerifyGetValueOrDefaultWhenKeyIsMissing(JsonObjectExtensions.GetNumberOrDefault);
        }

        [Fact]
        public void ItReturnsDefaultDateTimeWhenKeyIsMissing()
        {
            VerifyGetValueOrDefaultWhenKeyIsMissing(JsonObjectExtensions.GetDateTimeOrDefault);
        }

        [Fact]
        public void ItReturnsEmptyListAndAppendsItToObjectWhenKeyIsMissing()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            var result = jsonObj.GetListAndCreateIfMissing<JsonObject>("missingkey");

            //t
            result.Count.Should().Be(0);
            jsonObj.Count.Should().Be(1);
        }

        [Fact]
        public void ItReturnsEmptyListButDoeNotAppendItToObjectWhenKeyIsMissing()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            var result = jsonObj.GetList<JsonObject>("missingkey");

            //t
            result.Count.Should().Be(0);
            jsonObj.Count.Should().Be(0);
        }

        [Fact]
        public void ItReturnsEmptyStringWhenKeyIsMissing()
        {
            //g
            const string key = "somekey";
            const string expected = "";
            var jsonObj = new JsonObject();

            //w
            var actual = jsonObj.GetStringValueOrEmpty(key);

            //t
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItReturnsEmptyStringWhenRetrievingFirstReferenceValuesAndKeyIsMissing()
        {
            //g
            var jsonObj = new JsonObject {{"somekey", "someValue"}};

            //w
            var actual = jsonObj.GetFirstValueOrEmptyFromReference("some other key");

            //t
            actual.Should().Be("");
        }

        [Fact]
        public void ItReturnsFalseIfEtagDoesNotMatch()
        {
            //g
            var jsonObj = new JsonObject {{"etag", "123"}};

            //w
            var actual = jsonObj.ETagMatches("456");

            //t
            actual.Should().BeFalse();
        }

        [Fact]
        public void ItReturnsFalseIfEtagIsMissing()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            var actual = jsonObj.ETagMatches("123");

            //t
            actual.Should().BeFalse();
        }

        [Fact]
        public void ItReturnsFalseIfFirstResultIsNotEmpty()
        {
            //g
            var jsonObj = new JsonObject
            {
                {
                    "results", new List<JsonObject>
                    {new JsonObject {{"id", "123"}}}
                }
            };

            //w
            var actual = jsonObj.HasNoResults();

            //t
            actual.Should().BeFalse();
        }

        [Fact]
        public void ItReturnsFalseWhenKeyIsMissing()
        {
            VerifyGetValueOrDefaultWhenKeyIsMissing(JsonObjectExtensions.GetBoolOrFalse);
        }

        [Fact]
        public void ItReturnsFalse_WhenCheckingThePresenceOfReferenceFields_GivenReferenceFieldIsMissing()
        {
            //g
            var jsonObj = new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {Constants.Parentid, "123"},
            };

            //w
            var act = jsonObj.ContainsReferenceFields();

            //t
            act.Should().BeFalse();
        }

        [Fact]
        public void ItReturnsListOfElements_GivenIEnumerableStringInput()
        {
            // g
            var jsonObj = new JsonObject
            {
                {"values", new List<string> {"One", "Two"}}
            };

            // w
            var actual = jsonObj.GetValuesFromObject();

            // t
            actual.Should().HaveCount(2);
            actual.First().Should().Be("One");
        }

        [Fact]
        public void ItReturnsListOfElements_GivenObjectInput()
        {
            // g
            var jsonObj = new JsonObject
            {
                {"values", new [] {"One", "Two"}}
            };

            // w
            var actual = jsonObj.GetValuesFromObject();

            // t
            actual.Should().HaveCount(2);
            actual.First().Should().Be("One");
        }

        [Fact]
        public void ItReturnsListOfElements_GivenStringInput()
        {
            // g
            var jsonObj = new JsonObject
            {
                {"values", new[] {"One", "Two"}}
            };

            // w
            var actual = jsonObj.GetValuesFromObject();

            // t
            actual.Should().HaveCount(2);
            actual.First().Should().Be("One");
        }

        [Fact]
        public void ItReturnsTrueIfFirstResultIsEmpty()
        {
            //g
            var jsonObj = new JsonObject
            {
                {
                    "results", new List<JsonObject>
                    {new JsonObject()}
                }
            };

            //w
            var actual = jsonObj.HasNoResults();

            //t
            actual.Should().BeTrue();
        }

        [Fact]
        public void ItReturnsTrueIfResultAreEmpty()
        {
            //g
            var jsonObj = new JsonObject
            {
                {"results", new List<JsonObject>()}
            };

            //w
            var actual = jsonObj.HasNoResults();

            //t
            actual.Should().BeTrue();
        }

        [Fact]
        public void ItReturnsTrueIfResultsAreMissing()
        {
            //g
            var jsonObj = new JsonObject();

            //w
            var actual = jsonObj.HasNoResults();

            //t
            actual.Should().BeTrue();
        }

        [Fact]
        public void ItShouldRemoveTemporaryProperties()
        {
            // g
            const string tempKey = Constants.TemporaryPrefix + "duplicatekeys";
            var json = new JsonObject {{"id", "1000"}, {tempKey, 2}};

            // w
            json.RemoveTemporaryProperties();

            // t
            json.Id.Should().Be("1000");
            json.ContainsKey(tempKey).Should().BeFalse();
        }
    }
}