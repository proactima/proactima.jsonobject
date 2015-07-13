using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace proactima.jsonobject
{
    public partial class ImmutableJsonObject : IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return new XmlSchema();
        }

        public void ReadXml(XmlReader reader)
        {
            var asString = reader.ReadInnerXml();
            var obj = JsonObject.Parse(asString);
            _json = ImmutableDictionary(obj);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteRaw(JsonConvert.SerializeObject(this));
        }
    }
}