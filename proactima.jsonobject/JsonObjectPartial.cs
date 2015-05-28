using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace

namespace proactima.jsonobject
{
    [Serializable]
    public partial class JsonObject
    {
        protected JsonObject(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}