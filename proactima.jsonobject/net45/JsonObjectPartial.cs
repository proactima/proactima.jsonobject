using System;
using System.Runtime.Serialization;
using System.Text;
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
            var asEncoded = reader.ReadInnerXml();
            string asString;

            try
            {
                asString = Base64Decode(asEncoded);
            }
            catch (FormatException)
            {
                asString = asEncoded;
            }

            var asJson = Parse(asString);
            foreach (var kvp in asJson)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            var base64Encoded = Base64Encode(JsonConvert.SerializeObject(this));
            writer.WriteRaw(base64Encoded);
        }

        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.Unicode.GetString(base64EncodedBytes);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return new XmlSchema();
        }
    }
}