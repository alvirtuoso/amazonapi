using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AmazonProductSearch.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// Parses the xml. ***** Needs more testing
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="xml">Xml.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ParseXml<T>(string xml)
        {
            var obj = new Object();
            if (String.IsNullOrEmpty(xml)) throw new NotSupportedException("ERROR: input string cannot be null.");
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                using(StringReader stringReader = new StringReader(xml)){
					using (var reader = XmlReader.Create(stringReader))
					{
						return (T)xmlserializer.Deserialize(reader);
						
					}
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                throw new Exception("An error occurred", ex);
            }
     

        }

        /// <summary>
        /// Deserializes the xml.***** Needs more testing !!********
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="xml">Xml.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DeserializeXml<T>(XmlNode xml)
		{
			if (null == xml) throw new ArgumentNullException(nameof(xml), "Unable to serialize " + xml.ParentNode.InnerXml);
			T deserializedObject = default(T);
            try
            {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				
				using (StringReader stringReader = new StringReader(xml.OuterXml))
				{
					using(XmlReader xmlReader = XmlReader.Create(stringReader)){
						
						deserializedObject = (T)xmlSerializer.Deserialize(xmlReader);
					}
				}

            }
            catch (Exception ex)
            {
                var m = ex.Message;
                throw ex;
            }
			return deserializedObject;
		}

        /// <summary>
        /// Removes the name space. **** Needs more testing
        /// </summary>
        /// <returns>The name space.</returns>
        /// <param name="doc">Document.</param>
        public static XmlDocument RemoveNameSpace(XDocument doc)
        {
			foreach (var node in doc.Root.Descendants())
			{
				// If we have an empty namespace...
				if (node.Name.NamespaceName != "")
				{
					// Remove the xmlns='' attribute. Note the use of
					// Attributes rather than Attribute, in case the
					// attribute doesn't exist (which it might not if we'd
					// created the document "manually" instead of loading
					// it from a file.)
					node.Attributes("xmlns").Remove();
					// Inherit the parent namespace instead
					node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
				}
			}
			Console.WriteLine(doc); // Or doc.Save(...)
		
            return ConvertToXmlDocument(doc);
        }


        public static XmlDocument ConvertToXmlDocument(XDocument input)
        {
            var xmlDocumentObj = new XmlDocument();
            using (var xmlReader = input.CreateReader())
            {
                xmlDocumentObj.Load(xmlReader);
                return xmlDocumentObj;
            }
        }
    }
}