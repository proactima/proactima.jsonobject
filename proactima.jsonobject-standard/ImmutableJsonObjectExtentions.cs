using System;
using System.Collections.Generic;
using System.Linq;
using proactima.jsonobject.common;

namespace proactima.jsonobject
{
    public static class ImmutableJsonObjectExtentions
    {
        public static JsonObject ToMutable(this ImmutableJsonObject obj)
        {
            var result = new JsonObject();

	        foreach (var kvp in obj)
	        {
				var o = kvp.Value as ImmutableJsonObject;
		        if (o != null)
		        {
					result[kvp.Key] = o.ToMutable();
					continue;
		        }

				var asList = kvp.Value as List<ImmutableJsonObject>;
				if (asList != null)
				{
					result[kvp.Key] = asList.Select(imm => imm.ToMutable()).ToList();
					continue;
				}

				result[kvp.Key] = kvp.Value;
	        }

            return result;
        }

        public static ImmutableJsonObject CreateEntityReferenceField(this ImmutableJsonObject obj, string fieldName,
            string fieldType,
            IEnumerable<string> fieldValues)
        {
            return obj.CreateGenericReferenceField(fieldName, fieldType, fieldValues, Constants.EntityPrefix);
        }

        /// <summary>
        /// Creates an article reference field with name, type and values. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValues">Array of values</param>
        public static ImmutableJsonObject CreateArticleReferenceField(
            this ImmutableJsonObject obj,
            string fieldName,
            string fieldType,
            IEnumerable<string> fieldValues)
        {
            return obj.CreateGenericReferenceField(fieldName, fieldType, fieldValues, Constants.ArticlePrefix);
        }

        private static ImmutableJsonObject CreateGenericReferenceField(
            this ImmutableJsonObject obj,
            string fieldName,
            string fieldType,
            IEnumerable<string> fieldValues,
            string prefix)
        {
            var propertyName = prefix + fieldName + Constants.IdPostFix;
            var json = new JsonObject
            {
                {Constants.ReferenceType, fieldType},
                {Constants.ReferenceValues, fieldValues}
            };

            return obj.SetItem(propertyName, ImmutableJsonObject.FromMutable(json));
        }

        /// <summary>
        /// Creates an article reference field with name, type and value. Overwrites existing field with same name
        /// </summary>
        /// <param name="obj">The object to add the field to</param>
        /// <param name="fieldName">Name of the field. Don't include prefix/postfix</param>
        /// <param name="fieldType">The type of the reference used</param>
        /// <param name="fieldValue">Value</param>
        public static ImmutableJsonObject CreateArticleReferenceField(this ImmutableJsonObject obj, string fieldName,
            string fieldType,
            string fieldValue)
        {
            return obj.CreateGenericReferenceField(fieldName, fieldType, new[] {fieldValue}, Constants.ArticlePrefix);
        }

        /// <summary>
        /// Gets all the JsonObjects in the results collection
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns>All JsonObjects as an array. Returns empty array if no results present</returns>
        public static ImmutableJsonObject[] Results(this ImmutableJsonObject jsonObj)
        {
            return SharedExtensions.Results(jsonObj);
        }

        /// <summary>
        /// Get the value of a field. Returns string.empty on key missing
        /// </summary>
        /// <param name="obj">The object you want to get the field from</param>
        /// <param name="key">The field you want the value from</param>
        /// <returns></returns>
        public static object GetValue(this ImmutableJsonObject obj, string key)
        {
            return SharedExtensions.GetValue(obj, key);
        }

        /// <summary>
        /// Get the value of a field. Throws exception on conversion
        /// </summary>
        /// <typeparam name="TOut">Type to return as</typeparam>
        /// <param name="obj">The object you want to get the field from</param>
        /// <param name="key">The field you want the value from</param>
        /// <returns>Returns null if key missing.</returns>
        public static TOut GetValue<TOut>(this ImmutableJsonObject obj, string key) where TOut : class
        {
            return SharedExtensions.GetValue<TOut>(obj, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolOrFalse(this ImmutableJsonObject obj, string key)
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
        public static DateTime GetDateTimeOrDefault(this ImmutableJsonObject obj, string key)
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
        public static long GetNumberOrDefault(this ImmutableJsonObject obj, string key)
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
        public static ulong GetULongOrDefault(this ImmutableJsonObject obj, string key)
        {
            return SharedExtensions.GetULongOrDefault(obj, key);
        }

        /// <summary>
        /// Returns values from reference.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">Must have prefix/postfix</param>
        /// <returns>Returns object array of values, or empty array if not found</returns>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public static string[] GetValuesFromReference(this ImmutableJsonObject obj, string fieldName)
        {
            return SharedExtensions.GetValuesFromReference(obj, fieldName);
        }

        /// <summary>
        /// Returns values from the object, here object MUST be the inner object that has
        /// a values and a type key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string[] GetValuesFromObject(this ImmutableJsonObject obj)
        {
            return SharedExtensions.GetValuesFromObject(obj);
        }

        /// <summary>
        /// Returns first value from reference field or empty string if field is missing.
        /// </summary>
        public static string GetFirstValueOrEmptyFromReference(this ImmutableJsonObject obj,
            string fieldName)
        {
            return SharedExtensions.GetFirstValueOrEmptyFromReference(obj, fieldName);
        }

        /// <summary>
        /// Returns type of a reference.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">Must have prefix/postfix</param>
        /// <returns>Returns string.empty on missing</returns>
        public static string GetTypeFromReference(this ImmutableJsonObject obj, string fieldName)
        {
            return SharedExtensions.GetTypeFromReference(obj, fieldName);
        }

        /// <summary>
        /// Checks if JsonObject contains reference keys
        /// </summary>
        public static bool ContainsReferenceFields(this ImmutableJsonObject jsonObj)
        {
            return SharedExtensions.ContainsReferenceFields(jsonObj);
        }

        /// <summary>
        /// Get the value of a field as an IList. Returns an empty List on key missing (this empty list is NOT attached to the object)
        /// </summary>
        public static IList<TOut> GetList<TOut>(this ImmutableJsonObject obj, string key)
            where TOut : class
        {
            return SharedExtensions.GetList<TOut>(obj, key);
        }

        /// <summary>
        /// Returns IEnumerable of all reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, ImmutableJsonObject>> GetReferenceFields(
            this IEnumerable<KeyValuePair<string, object>> jsonObject)
        {
            if (jsonObject == null) return new[] {new KeyValuePair<string, ImmutableJsonObject>()};

            return jsonObject.Where(d => d.Key.EndsWith(Constants.IdPostFix))
                .Select(
                    (pair, i) =>
                        new KeyValuePair<string, ImmutableJsonObject>(pair.Key, pair.Value as ImmutableJsonObject));
        }


        /// <summary>
        /// Returns IEnumerable of all Article reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, ImmutableJsonObject>> GetArticleReferences(this ImmutableJsonObject currentObject)
        {
            return GetReferenceFields(currentObject, Constants.ArticlePrefix);
        }

        /// <summary>
        /// Returns IEnumerable of all Entity reference fields
        /// </summary>
        public static IEnumerable<KeyValuePair<string, ImmutableJsonObject>> GetEntityReferences(this ImmutableJsonObject currentObject)
        {
            return GetReferenceFields(currentObject, Constants.EntityPrefix);
        }

        private static IEnumerable<KeyValuePair<string, ImmutableJsonObject>> GetReferenceFields(
            ImmutableJsonObject currentObject,
            string prefix)
        {
            var query = currentObject
                .GetReferenceFields()
                .Where(kvp => kvp.Key.StartsWith(prefix));
            return query;
        }

        /// <summary>
        /// Get the value of a field or string.empty on missing key.
        /// </summary>
        /// <param name="obj">The object you want to get the value from</param>
        /// <param name="key">The key you want the value from</param>
        /// <returns>Returns string.empty if key is missing.</returns>
        public static string GetStringValueOrEmpty(this ImmutableJsonObject obj, string key)
        {
            return SharedExtensions.GetStringValueOrEmpty(obj, key);
        }
    }
}