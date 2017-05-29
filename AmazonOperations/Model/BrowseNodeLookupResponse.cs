using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    [XmlRoot]
    public class BrowseNodeLookupResponse : AmazonResponse
    {
        public BrowseNodes BrowseNodes { get; set; }
    }
}
