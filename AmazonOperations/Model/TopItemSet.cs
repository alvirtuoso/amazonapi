using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    [XmlRoot]
    public class TopItemSet
    {
        public string Type { get; set; }
        [XmlElement("TopItem")]
        public TopItem[] TopItem { get; set; }
    }
}
