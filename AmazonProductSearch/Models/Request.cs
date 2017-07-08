using System.Xml.Serialization;

namespace AmazonProductSearch.Models
{
    public class Request
    {
        public bool IsValid { get; set; }
        //[XmlArrayItem("Error")]
        //public AmazonError[] Errors { get; set; }
    }
}
