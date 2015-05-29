using Newtonsoft.Json.Linq;
using proactima.jsonobject.common;

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

        internal static JsonObject CreateArticleWithChildReference(string[] childIds, string id = "1",
            string parentId = "1",
            string name = "One")
        {
            var dict = new JsonObject
            {
                {Constants.Id, id},
                {Constants.Parentid, parentId},
                {Constants.Name, name},
                {"a_seahorse_ids", new JsonObject {{"type", "seahorse"}, {"values", childIds}}},
            };

            return dict;
        }

        public static JsonObject CreateSimpleCompleteObject(string id)
        {
            return new JsonObject
            {
                {Constants.Id, id},
                {Constants.Name, "testName"}
            };
        }

        public static JsonObject CreateObjectWithChildren()
        {
            return new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {
                    "a_risk_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                }
            };
        }

        public static JsonObject CreateLargeObjectWithChildren()
        {
            return new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {System.Guid.NewGuid().ToString(), "testName"},
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    System.Guid.NewGuid().ToString(), new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_a_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_b_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_c_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_d_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_e_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_f_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                },
                {
                    "a_g_ids", new JsonObject
                    {
                        {"type", "risk"},
                        {"values", new object[] {"1", "2"}}
                    }
                }
            };
        }

        public static JsonObject CreateObjectWithoutId()
        {
            return new JsonObject
            {
                {Constants.Name, "testName"}
            };
        }

        public static JsonObject CreateArticleWithJArray()
        {
            return new JsonObject
            {
                {Constants.Id, "1"},
                {Constants.Name, "testName"},
                {
                    "concerns", new JArray {new JObject {{"id", "someid!"}}}
                }
            };
        }
    }
}