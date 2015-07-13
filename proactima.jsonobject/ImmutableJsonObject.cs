﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace proactima.jsonobject
{
    public class ImmutableJsonObject : IReadOnlyDictionary<string, object>
    {
        private static ImmutableJsonObject FromImmutable(
            IImmutableDictionary<string, object> dict)
        {
            return new ImmutableJsonObject(dict);
        }
        public static ImmutableJsonObject FromMutable(JsonObject obj)
        {
            var immutable = obj.ToImmutableDictionary();
            foreach (var k in immutable.Keys)
            {
                var asList = immutable[k] as List<JsonObject>;
                if (asList == null)
                {
                    var asArray = immutable[k] as JsonObject[];
                    if (asArray == null)
                    {
                        var o = immutable[k] as JsonObject;
                        if (o == null) continue;

                        immutable = immutable.SetItem(k, FromMutable(o));
                    }
                    else
                    {
                        immutable = SetList(asArray, immutable, k);
                    }
                }
                else
                {
                    immutable = SetList(asList, immutable, k);
                }
            }

            return new ImmutableJsonObject(immutable);
        }

        private ImmutableJsonObject(IImmutableDictionary<string, object> json)
        {
            _json = json;
        }

        private static ImmutableDictionary<string, object> SetList(IEnumerable<JsonObject> keyList,
            ImmutableDictionary<string, object> immutable, string k)
        {
            var im = keyList.Select(FromMutable).ToList();
            immutable = immutable.SetItem(k, im);
            return immutable;
        }

        private readonly IImmutableDictionary<string, object> _json;

        public object this[string key]
        {
            get { return _json[key]; }
        }

        public int Count
        {
            get { return _json.Count; }
        }

        public IEnumerable<string> Keys
        {
            get { return _json.Keys; }
        }

        public IEnumerable<object> Values
        {
            get { return _json.Values; }
        }

        public ImmutableJsonObject Add(string key, object value)
        {
            return FromImmutable(_json.Add(key, value));
        }

        public ImmutableJsonObject AddRange(IEnumerable<KeyValuePair<string, object>> pairs)
        {
            return FromImmutable(_json.AddRange(pairs));
        }

        public ImmutableJsonObject Clear()
        {
            return FromImmutable(_json.Clear());
        }
        

        public bool ContainsKey(string key)
        {
            return _json.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _json.GetEnumerator();
        }

        public ImmutableJsonObject Remove(string key)
        {
            return FromImmutable(_json.Remove(key));
        }

        public ImmutableJsonObject SetItem(string key, object value)
        {
            return FromImmutable(_json.SetItem(key, value));
        }

        public ImmutableJsonObject SetItems(IEnumerable<KeyValuePair<string, object>> items)
        {
            return FromImmutable(_json.SetItems(items));
        }

        public bool TryGetKey(string equalKey, out string actualKey)
        {
            return _json.TryGetKey(equalKey, out actualKey);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _json.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _json.GetEnumerator();
        }
    }
}