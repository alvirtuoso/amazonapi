using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    public class Request
    {
        public string IsValid { get; set; }
        [XmlArrayItem("Error")]
        public AmazonError[] Errors { get; set; }
    }
}
