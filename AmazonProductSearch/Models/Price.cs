using System.Xml.Serialization;

namespace AmazonProductSearch.Models
{
    public class Price
    {
        [XmlElement(DataType = "integer")]
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FormattedPrice { get; set; }
    }
}
