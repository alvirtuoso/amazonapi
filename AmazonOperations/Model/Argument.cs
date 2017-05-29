using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    public class Argument
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
