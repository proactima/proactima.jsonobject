using System;

namespace proactima.jsonobject.tests
{
    public static class JsonFactory
    {
        internal static JsonObject CreateMiniEntity(string id = "1", string parentId = "1",
            string name = "One", string etag = "some_1")
        {
            var dict = new JsonObject
            {
                {Constants.Id, id},
                {Constants.Parentid, parentId},
                {Constants.Name, name},
                {"ETag", etag}
            };

            return dict;
        }

        public static JsonObject CreateComplexJsonObject()
        {
            return new JsonObject
            {
                {"a", "b"},
                {"b", true},
                {"c", 1},
                {"d", DateTime.UtcNow},
                {
                    "e", new JsonObject
                    {
                        {"1", "typ"},
                        {"2", new[] {"a", "b"}}
                    }
                },
            };
        }

        public static JsonObject CreateObjectWithoutId()
        {
            return new JsonObject
            {
                {Constants.Name, "testName"}
            };
        }
    }
}