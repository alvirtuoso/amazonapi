using System.Xml.Serialization;

namespace AmazonOperations.Model
{
    public class LoyaltyPoints
    {
        [XmlElement(DataType = "nonNegativeInteger")]
        public string Points { get; set; }
        public Price TypicalRedemptionValue { get; set; }
    }
}
