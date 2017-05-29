using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    [XmlRoot]
    public class TopSellers
    {
        [XmlElement("TopSeller")]
        public TopSeller[] TopSeller { get; set; }
    }
}
