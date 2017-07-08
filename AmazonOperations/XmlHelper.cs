using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AmazonOperations
{
    public static class XmlHelper
    {

        public static T ParseXml<T>(string xml)
        {
            var obj = new Object();
            if (String.IsNullOrEmpty(xml)) throw new NotSupportedException("ERROR: input string cannot be null.");
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringReader = new StringReader(xml);
                using (var reader = XmlReader.Create(stringReader))
                {
                    return (T)xmlserializer.Deserialize(reader);
                   
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                throw new Exception("An error occurred", ex);
            }
     

        }
    }
}