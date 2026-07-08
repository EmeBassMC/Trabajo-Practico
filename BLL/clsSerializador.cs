using System.IO;
using System.Xml.Serialization;

namespace BLL
{
    public static class clsSerializador
    {
        public static string Serializar<T>(T objeto)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, objeto);
                return sw.ToString();
            }
        }

        public static T Deserializar<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xml))
            {
                return (T)serializer.Deserialize(sr);
            }
        }
    }
}