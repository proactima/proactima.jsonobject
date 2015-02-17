using System.IO;
using Polenter.Serialization;

namespace proactima.jsonobject
{
    public static class UXSerializer
    {
        private static readonly SharpSerializer Serializer =
            new SharpSerializer(new SharpSerializerBinarySettings(BinarySerializationMode.SizeOptimized));

        public static Stream Serialize(object obj)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(obj, ms);
            ms.Position = 0;
            return ms;
        }

        public static T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            var deserialized = (T)Serializer.Deserialize(stream);
            return deserialized;
        }
    }
}