using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace

namespace proactima.jsonobject
{
    [Serializable]
    public partial class JsonObject : IXmlSerializable
    {
        protected JsonObject(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public void ReadXml(XmlReader reader)
        {
            var asString = reader.ReadInnerXml();
            var asJson = Parse(asString);
            foreach (var kvp in asJson)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteRaw(JsonConvert.SerializeObject(this));
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return new XmlSchema();
        }
    }
}