using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace proactima.jsonobject
{
    [Serializable]
    public partial class ImmutableJsonObject : IXmlSerializable
    {
        public void ReadXml(XmlReader reader)
        {
            var asEncoded = reader.ReadInnerXml();
            string asString;

            try
            {
                asString = EncodingHelper.Base64Decode(asEncoded);
            }
            catch (FormatException)
            {
                asString = asEncoded;
            }

            var asJson = JsonObject.Parse(asString, true);
            _json = ImmutableDictionary(asJson);
        }

        public void WriteXml(XmlWriter writer)
        {
            var base64Encoded = EncodingHelper.Base64Encode(JsonConvert.SerializeObject(this));
            writer.WriteRaw(base64Encoded);
        }


        XmlSchema IXmlSerializable.GetSchema()
        {
            return new XmlSchema();
        }

        public XmlSchema GetSchema()
        {
            return new XmlSchema();
        }
    }
}