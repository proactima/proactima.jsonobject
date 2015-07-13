using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using proactima.jsonobject.common;

namespace proactima.jsonobject
{
    public partial class JsonObject : Dictionary<string, object>, IReadOnlyDictionary<string, object>
    {
        public JsonObject()
        {
        }

        public string Id
        {
            get
            {
                EnsureHasKey(Constants.Id);
                return base[Constants.Id].ToString();
            }
        }

        public string ParentId
        {
            get
            {
                EnsureHasKey(Constants.Parentid);
                return this[Constants.Parentid].ToString();
            }
        }

        public string ParentType
        {
            get
            {
                EnsureHasKey(Constants.ParentType, String.Empty);
                return this[Constants.ParentType].ToString();
            }
        }

        public bool IsRootObject
        {
            get { return ParentId.Equals(Constants.NullId); }
        }

        public new object this[string key]
        {
            get { return base[key.ToLowerInvariant()]; }
            set { base[key.ToLowerInvariant()] = value; }
        }

        public bool HasParent()
        {
            return ParentId != Constants.NullId && !string.IsNullOrEmpty(ParentType);
        }


        private void EnsureHasKey(string key, string defaultValue = "0")
        {
            if (!ContainsKey(key) ||
                this[key] == null ||
                String.IsNullOrEmpty(this[key].ToString()))
                this[key] = defaultValue;
        }

        public new void Add(string key, object value)
        {
            base.Add(key.ToLowerInvariant(), value);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, object>> objects)
        {
            foreach (var kvp in objects)
                this[kvp.Key] = kvp.Value;
        }

        public static JsonObject Parse(string content, bool keepGeneratedContent = false)
        {
            return FromJObject(JObject.Parse(content), keepGeneratedContent);
        }

        public static JsonObject FromJObject(JObject obj, bool keepGeneratedContent = false)
        {
            var json = new JsonObject();
            foreach (var valuePair in obj)
            {
                var type = valuePair.Value.Type.ToString().ToLowerInvariant();
                var key = valuePair.Key;

                if (!keepGeneratedContent &&
                    (key.StartsWith(Constants.GeneratedArticlePrefix) ||
                     key.StartsWith(Constants.GeneratedEntityPrefix)))
                    continue;

                switch (type)
                {
                    case "object":
                        json.Add(key, FromJObject((JObject)valuePair.Value, keepGeneratedContent));
                        break;
                    case "array":
                        var array = (JArray) valuePair.Value;

                        if (array.Count == 0)
                        {
                            json.Add(key, new object[0]);
                            break;
                        }

                        // is this array an object array or value array?
                        var isValues = array.First().GetType() == typeof (JValue);
                        if (isValues)
                        {
                            json.Add(key, array.Select(t => ReadValueByType(t.Type, t)).ToArray());
                        }
                        else
                        {
                            var children = (from JObject val in array select FromJObject(val, keepGeneratedContent)).ToList();
                            json.Add(key, children);
                        }
                        break;
                    default:
                        json.Add(key, ReadValueByType(valuePair.Value.Type, valuePair.Value));
                        break;
                }
            }

            return json;
        }

        private static object ReadValueByType(JTokenType currentType, IEnumerable<JToken> token)
        {
            switch (currentType.ToString().ToLowerInvariant())
            {
                case "null":
                    return null;
                case "boolean":
                    return token.Value<Boolean>();
                case "int32":
                case "int64":
                case "integer":
                case "long":
                    return token.Value<long>();
                case "float":
                case "decimal":
                case "double":
                case "number":
                    return token.Value<decimal>();
                case "date":
                case "datetime":
                    return token.Value<DateTime>();
                default:
                    var value = token.Value<string>();
                    if (String.IsNullOrEmpty(value))
                        value = string.Empty;
                    return value;
            }
        }


        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}