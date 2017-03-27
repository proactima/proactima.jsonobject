using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using proactima.jsonobject.common;

namespace proactima.jsonobject
{
    internal static class SharedExtensions
    {
        internal static T[] Results<T>(T jsonObj) where T : class, IReadOnlyDictionary<string, object>
        {
            if (!jsonObj.ContainsKey(Constants.Results)) return new T[0];

            var results = jsonObj[Constants.Results];
            if (results is T)
                return new[] {results as T};
            if (results is T[])
                return results as T[];
            if (results is IEnumerable<T>)
                return (results as IEnumerable<T>).ToArray();

            return new T[0];
        }

        internal static object GetValue(IReadOnlyDictionary<string, object> obj, string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var lowerKey = key.ToLowerInvariant();

            return !obj.ContainsKey(lowerKey) ? String.Empty : obj[lowerKey];
        }

        internal static TOut GetValue<TOut>(IReadOnlyDictionary<string, object> obj, string key) where TOut : class
        {
            var value = GetValue(obj, key) as TOut;

            if (value == null)
                throw new InvalidOperationException(String.Format("Cannot convert the value of '{0}' to '{1}'", key,
                    typeof(TOut)));

            return value;
        }

        internal static string GetStringValueOrEmpty(IReadOnlyDictionary<string, object> obj, string key)
        {
            var lowerKey = key.ToLowerInvariant();

            if (!obj.ContainsKey(lowerKey))
                return String.Empty;

            return obj[lowerKey] == null ? String.Empty : obj[lowerKey].ToString();
        }

        internal static DateTime GetDateTimeOrDefault(IReadOnlyDictionary<string, object> obj, string key)
        {
            var dateAsString = GetStringValueOrEmpty(obj, key);
            if (String.IsNullOrEmpty(dateAsString))
                return default(DateTime);

            DateTime parsedDateTime;
            var couldParse = DateTime.TryParse(dateAsString, out parsedDateTime);
            return couldParse
                ? parsedDateTime
                : default(DateTime);
        }

        internal static long GetNumberOrDefault(IReadOnlyDictionary<string, object> obj, string key)
        {
            var lowerKey = key.ToLowerInvariant();
            const long defaultValue = default(long);

            if (!obj.ContainsKey(lowerKey))
                return defaultValue;

            var value = obj[lowerKey];

            if (value is int)
                return (int)obj[lowerKey];

            if (value is long)
                return (long)obj[lowerKey];

            if (value is string)
            {
                long parsedValue;
                if (Int64.TryParse(value.ToString(), out parsedValue))
                    return parsedValue;
            }

            return defaultValue;
        }

        internal static ulong GetULongOrDefault(IReadOnlyDictionary<string, object> obj, string key)
        {
            var lowerKey = key.ToLowerInvariant();
            const ulong defaultValue = default(ulong);

            if (!obj.ContainsKey(lowerKey))
                return defaultValue;

            var value = obj[lowerKey];

            if (value is int)
                return Convert.ToUInt64((int)obj[lowerKey]);

            if (value is long)
                return Convert.ToUInt64((long)obj[lowerKey]);

            if (value is ulong)
                return (ulong)obj[lowerKey];

            if (value is string)
            {
                ulong parsedValue;
                if (UInt64.TryParse(value.ToString(), out parsedValue))
                    return parsedValue;
            }

            return defaultValue;
        }

        internal static string[] GetValuesFromObject(IReadOnlyDictionary<string, object> obj)
        {
            if (!obj.ContainsKey(Constants.ReferenceValues)) return new string[0];
            var values = GetList<string>(obj, Constants.ReferenceValues);
            return values == null
                ? new string[0]
                : values.ToArray();
        }

        internal static IList<TOut> GetList<TOut>(IReadOnlyDictionary<string, object> obj, string key)
           where TOut : class
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var list = GetValue(obj,key) as IList<TOut>;

            if (list != null)
                return list;

            var enumerable = GetValue(obj, key) as IEnumerable<TOut>;
            if (enumerable != null) return enumerable.ToList();

            var objects = GetValue(obj, key) as object[];
            return objects != null && objects.All(o => (o as TOut) != null)
                ? objects.Select(o => o as TOut).ToList()
                : new List<TOut>();
        }

        internal static string[] GetValuesFromReference(IReadOnlyDictionary<string, object> obj, string fieldName)
        {
            if (!obj.ContainsKey(fieldName)) return new string[0];

            var innerObject = obj[fieldName] as IReadOnlyDictionary<string, object>;
            if (innerObject != null) return GetValuesFromObject(innerObject);

            var innerJObject = obj[fieldName] as JObject;
            if (innerJObject != null)
                innerObject = JsonObject.FromJObject(innerJObject);

            return (innerObject == null)
                ? new string[0]
                : GetValuesFromObject(innerObject);
        }

        internal static T GetValueOrDefault<T>(IReadOnlyDictionary<string, object> obj, string key) where T : struct
        {
            var lowerKey = key.ToLowerInvariant();

            if (!obj.ContainsKey(lowerKey))
                return default(T);

            return obj[lowerKey] == null ? default(T) : (T)obj[lowerKey];
        }

        internal static string GetFirstValueOrEmptyFromReference(IReadOnlyDictionary<string, object> obj,
           string fieldName)
        {
            if (!obj.ContainsKey(fieldName)) return String.Empty;

            var objValue = GetValuesFromReference(obj, fieldName).FirstOrDefault();
            return objValue ?? String.Empty;
        }

        internal static string GetTypeFromReference(IReadOnlyDictionary<string, object> obj, string fieldName)
        {
            var innerObject = obj[fieldName] as IReadOnlyDictionary<string, object>;
            if (innerObject != null) return innerObject[Constants.ReferenceType] as string ?? String.Empty;

            var innerJObject = obj[fieldName] as JObject;
            innerObject = JsonObject.FromJObject(innerJObject);

            return (innerObject == null)
                ? String.Empty
                : innerObject[Constants.ReferenceType] as string ?? String.Empty;
        }

   
        internal static bool ContainsReferenceFields(IEnumerable<KeyValuePair<string, object>> jsonObj)
        {
            return jsonObj.Any(d => d.Key.EndsWith(Constants.IdPostFix));
        }
    }
}