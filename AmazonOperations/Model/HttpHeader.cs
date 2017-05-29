using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    public class HttpHeader
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
