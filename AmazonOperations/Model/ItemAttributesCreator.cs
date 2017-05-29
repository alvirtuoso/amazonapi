using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    public class ItemAttributesCreator
    {
        [XmlAttribute]
        public string Role { get; set; }
        [XmlText]
        public string Name { get; set; }
    }
}
