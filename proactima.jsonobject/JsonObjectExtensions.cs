using System;
using System.Collections.Generic;
using System.Linq;
using proactima.jsonobject.common;

namespace proactima.jsonobject
{
    public static class JsonObjectExtensions
    {
        /// <summary>
        /// Creates a new JsonObject and copies over the data from the field  
        /// names specified.
        /// </summary>
        /// <param name="original">The to-be-cloned object</param>
        /// <param name="legalFieldNames">Fields/Properties to copy values to/from</param>
        /// <returns></returns>
        public static JsonObject CloneKeepingOnly(this JsonObject original, IEnumerable<string> legalFieldNames)
        {
            var cloned = new JsonObject();
            original
                .Where(kvp => legalFieldNames.Any(field => string.Equals(field, kvp.Key)))
                .ToList()
                .ForEach(kvp => cloned.Add(kvp.Key, kvp.Value));

            return cloned;
        }

        /// <summary>
        /// Get the value of a field. Returns string.empty on key missing
        /// </summary>
        /// <param name="obj">The object you want to get the field from</param>
        /// <param name="key">The field you want the value from</param>
        /// <returns></returns>
        public static object GetValue(this JsonObject obj, string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var lowerKey = key.ToLowerInvariant();

            return !obj.ContainsKey(lowerKey) ? String.Empty : obj[lowerKey];
        }


        /// <summary>
        /// Creates an article reference field with name, type and values. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValues">Array of values</param>
        public static void CreateArticleReferenceField(this JsonObject obj, string fieldName, string fieldType,
            IEnumerable<string> fieldValues)
        {
            obj.CreateGenericReferenceField(fieldName, fieldType, fieldValues, Constants.ArticlePrefix);
        }

        /// <summary>
        /// Creates an article reference field with name, type and values. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="propertyName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValues">Array of values</param>
        public static void CreateReferenceField(this JsonObject obj, string propertyName, string fieldType,
            IEnumerable<string> fieldValues)
        {
            obj[propertyName] = new JsonObject
            {
                {Constants.ReferenceType, fieldType},
                {Constants.ReferenceValues, fieldValues}
            };
        }

        /// <summary>
        /// Returns an entity reference field name.
        /// <para>E.g. given a string 'user' it will return 'e_user_ids'.</para>
        /// </summary>
        public static string GenerateEntityReferenceName(this string fieldName)
        {
            return Constants.EntityPrefix + fieldName + Constants.IdPostFix;
        }

        private static void CreateGenericReferenceField(this JsonObject obj, string fieldName, string fieldType,
            IEnumerable<string> fieldValues, string prefix)
        {
            var propertyName = prefix + fieldName + Constants.IdPostFix;
            obj.CreateReferenceField(propertyName, fieldType, fieldValues);
        }

        /// <summary>
        /// Creates an article reference field with name, type and value. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValue">Value</param>
        public static void CreateArticleReferenceField(this JsonObject obj, string fieldName, string fieldType,
            string fieldValue)
        {
            obj.CreateGenericReferenceField(fieldName, fieldType, new[] {fieldValue}, Constants.ArticlePrefix);
        }

        /// <summary>
        /// Creates an entity reference field with name, type and values. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValues">Array of values</param>
        public static void CreateEntityReferenceField(this JsonObject obj, string fieldName, string fieldType,
            IEnumerable<string> fieldValues)
        {
            obj.CreateGenericReferenceField(fieldName, fieldType, fieldValues, Constants.EntityPrefix);
        }

        /// <summary>
        /// Creates an entity reference field with name, type and value. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValue">Value</param>
        public static void CreateEntityReferenceField(this JsonObject obj, string fieldName, string fieldType,
            string fieldValue)
        {
            obj.CreateGenericReferenceField(fieldName, fieldType, new[] {fieldValue}, Constants.EntityPrefix);
        }

        /// <summary>
        /// Get the value of a field. Throws exception on conversion
        /// </summary>
        /// <typeparam name="TOut">Type to return as</typeparam>
        /// <param name="obj">The object you want to get the field from</param>
        /// <param name="key">The field you want the value from</param>
        /// <returns>Returns null if key missing.</returns>
        public static TOut GetValue<TOut>(this JsonObject obj, string key) where TOut : class
        {
            return SharedExtensions.GetValue<TOut>(obj, key);
        }

        /// <summary>
        /// Get the value of a field or string.empty on missing key.
        /// </summary>
        /// <param name="obj">The object you want to get the value from</param>
        /// <param name="key">The key you want the value from</param>
        /// <returns>Returns string.empty if key is missing.</returns>
        public static string GetStringValueOrEmpty(this JsonObject obj, string key)
        {
            return SharedExtensions.GetStringValueOrEmpty(obj, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolOrFalse(this JsonObject obj, string key)
        {
            return SharedExtensions.GetValueOrDefault<bool>(obj, key);
        }

        /// <summary>
        /// Will attempt to get the value from key and parse to 
        /// DateTime. If it fails at any point a default date will be returned.
        /// </summary>
        /// <param name="obj">The JsonObject to get the value from</param>
        /// <param name="key">The Key for the value (ie. Deadline)</param>
        /// <returns>DateTime, default or parsed</returns>
        public static DateTime GetDateTimeOrDefault(this JsonObject obj, string key)
        {
            return SharedExtensions.GetDateTimeOrDefault(obj, key);
        }

        /// <summary>
        /// Gets a long from a JsonObject.
        /// <para>The value being retrieved can be stored in the JsonObject as an int, long or string.</para>
        /// <para>If the key is missing or the value is some other type, 0 is returned.</para>
        /// </summary>
        /// <param name="obj">The object you want to get the value from</param>
        /// <param name="key">The key you want the value from</param>
        /// <returns>Returns a long or 0 if key is missing</returns>
        public static long GetNumberOrDefault(this JsonObject obj, string key)
        {
            return SharedExtensions.GetNumberOrDefault(obj, key);
        }

        /// <summary>
        /// Gets a ulong from a JsonObject.
        /// <para>The value being retrieved can be stored in the JsonObject as an int, long, ulong or string.</para>
        /// <para>If the key is missing or the value is some other type, 0 is returned.</para>
        /// </summary>
        /// <param name="obj">The object you want to get the value from</param>
        /// <param name="key">The key you want the value from</param>
        /// <returns>Returns a ulong or 0 if key is missing</returns>
        public static ulong GetULongOrDefault(this JsonObject obj, string key)
        {
            return SharedExtensions.GetULongOrDefault(obj, key);
        }

        /// <summary>
        /// Returns first result from the results collection.
        /// Unsafe method, can easily blow up...
        /// </summary>
        /// <param name="jObj"></param>
        /// <returns></returns>
        public static JsonObject FirstResult(this JsonObject jObj)
        {
            if (!jObj.ContainsKey(Constants.Results) || !jObj.Results().Any()) return new JsonObject();

            return jObj.Results()[0];
        }

        /// <summary>
        /// Gets all the JsonObjects in the results collection
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns>All JsonObjects as an array. Returns empty array if no results present</returns>
        public static JsonObject[] Results(this JsonObject jsonObj)
        {
            return SharedExtensions.Results(jsonObj);
        }

        /// <summary>
        /// Returns values from reference.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">Must have prefix/postfix</param>
        /// <returns>Returns object array of values, or empty array if not found</returns>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public static string[] GetValuesFromReference(this JsonObject obj, string fieldName)
        {
            return SharedExtensions.GetValuesFromReference(obj, fieldName);
        }

        /// <summary>
        /// Returns values from the object, here object MUST be the inner object that has
        /// a values and a type key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string[] GetValuesFromObject(this JsonObject obj)
        {
            return SharedExtensions.GetValuesFromObject(obj);
        }

        /// <summary>
        /// Returns first value from reference field or empty string if field is missing.
        /// </summary>
        public static string GetFirstValueOrEmptyFromReference(this JsonObject obj, string fieldName)
        {
            return SharedExtensions.GetFirstValueOrEmptyFromReference(obj, fieldName);
        }

        /// <summary>
        /// Returns type of a reference.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">Must have prefix/postfix</param>
        /// <returns>Returns string.empty on missing</returns>
        public static string GetTypeFromReference(this JsonObject obj, string fieldName)
        {
            return SharedExtensions.GetTypeFromReference(obj, fieldName);
        }

        /// <summary>
        /// Checks if JsonObject contains reference keys
        /// </summary>
        public static bool ContainsReferenceFields(this IEnumerable<KeyValuePair<string, object>> jsonObj)
        {
            return SharedExtensions.ContainsReferenceFields(jsonObj);
        }

        /// <summary>
        /// Returns IEnumerable of all Article reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, JsonObject>> GetArticleReferences(this JsonObject currentObject)
        {
            return GetReferenceFields(currentObject, Constants.ArticlePrefix);
        }

        /// <summary>
        /// Returns IEnumerable of all Entity reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, JsonObject>> GetEntityReferences(this JsonObject currentObject)
        {
            return GetReferenceFields(currentObject, Constants.EntityPrefix);
        }

        private static IEnumerable<KeyValuePair<string, JsonObject>> GetReferenceFields(
            JsonObject currentObject,
            string prefix)
        {
            var query = currentObject
                .GetReferenceFields()
                .Where(kvp => kvp.Key.StartsWith(prefix));
            return query;
        }

        /// <summary>
        /// Returns IEnumerable of all reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, JsonObject>> GetReferenceFields(this JsonObject jsonObject)
        {
            if (jsonObject == null) return new[] {new KeyValuePair<string, JsonObject>()};

            return jsonObject.Where(d => d.Key.EndsWith(Constants.IdPostFix))
                .Select((pair, i) => new KeyValuePair<string, JsonObject>(pair.Key, pair.Value as JsonObject));
        }

        /// <summary>
        /// Copies sys_* properties from parent to this.
        /// </summary>
        public static void SetSystemPropertiesFrom(this JsonObject jsonObject, JsonObject parent)
        {
            var properties = from p in parent
                where p.Key.StartsWith(Constants.SystemPrefix)
                select p;

            foreach (var property in properties)
            {
                jsonObject[property.Key] = property.Value;
            }
        }

        /// <summary>
        /// Shallow clone of JsonObject
        /// </summary>
        public static JsonObject Clone(this JsonObject json)
        {
            var cloned = new JsonObject();
            json.ToList()
                .ForEach(kvp =>
                {
                    var o = kvp.Value as JsonObject;
                    if (o != null)
                        cloned[kvp.Key] = o.Clone();
                    else
                        cloned[kvp.Key] = kvp.Value;
                });

            return cloned;
        }

        /// <summary>
        /// Get the value of a field as an IList. Returns an empty List on key missing (this empty list is NOT attached to the object)
        /// </summary>
        public static IList<TOut> GetList<TOut>(this JsonObject obj, string key) where TOut : class
        {
            return SharedExtensions.GetList<TOut>(obj, key);
        }

        /// <summary>
        /// Get the value of a field as an IList. Returns an empty List on key missing (this empty list is attached to the object)
        /// </summary>
        public static IList<TOut> GetListAndCreateIfMissing<TOut>(this JsonObject obj, string key) where TOut : class
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (!obj.ContainsKey(key))
                obj.Add(key, new List<TOut>());

            var list = obj.GetValue(key) as IList<TOut>;

            if (list == null)
                throw new ArgumentException(String.Format("Key '{0}' is not IList in given JsonObject.", key));

            return list;
        }

        /// <summary>
        /// Removes all the keys starting with "tmp_" from the JsonObject
        /// </summary>
        public static void RemoveTemporaryProperties(this JsonObject json)
        {
            var a = json.Where(k => k.Key.StartsWith(Constants.TemporaryPrefix)).Select(i => i.Key).ToList();
            if (a.Any())
                a.ForEach(i => json.Remove(i));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="eTag"></param>
        /// <returns></returns>
        public static bool FirstResultETagMatches(this JsonObject jObj, string eTag)
        {
            if (String.IsNullOrEmpty(eTag)) return false;

            var firstResult = jObj.FirstResult();

            return firstResult.ETagMatches(eTag);
        }

        /// <summary>
        /// Checks if the current objects' etag matches specified etag
        /// </summary>
        /// <param name="jObj">Current object</param>
        /// <param name="eTag">Comparable etag</param>
        /// <returns></returns>
        public static bool ETagMatches(this JsonObject jObj, string eTag)
        {
            return jObj.ContainsKey("etag") && jObj["etag"].ToString() == eTag;
        }

        /// <summary>
        /// Checks if there are any properties on the object,
        /// if there are will also check for Results collection.
        /// </summary>
        /// <param name="json">Current object</param>
        /// <returns></returns>
        public static bool HasNoResults(this JsonObject json)
        {
            if (!json.Any())
                return true;

            return !json.Results().Any() ||
                   json.Results().First().Id.Equals(Constants.NullId);
        }
    }
}