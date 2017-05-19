using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Net.Http;
using AmazonProductSearch.Models;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
//using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmazonProductSearch.Helpers
{
    public static class HtmlHelper
    {
        public static Boolean IsPrimeEligible(XmlNodeList isPrimeEligibleNode){
            var result = false;
            if (isPrimeEligibleNode != null && isPrimeEligibleNode.Count > 0)
            {
                var text = isPrimeEligibleNode[0].InnerText;
                if (!string.IsNullOrEmpty(text) && text.Equals("1"))
                {
                    result = true;
                }
            }
            return result;
        }

		/// <summary>
		/// Finds the XmlNode from it's parent node specified by it's name.
		/// </summary>
		/// <returns>The node.</returns>
		/// <param name="nodeName">Node name.</param>
		/// <param name="node">Node.</param>
		public static XmlNode FindNode(string nodeName, XmlNode node)
		{
			XmlNode n = null;
			var nodes = node.ChildNodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].Name == nodeName)
				{
					n = nodes[i];
					break;
				}
			}
			return n;
		}

        public static List<String> ItemFeatures(XmlNode attributesNode)
		{
            List<String> features = new List<string>();
            if (attributesNode != null)
				for (int i = 0; i < attributesNode.ChildNodes.Count; i++)
				{
					if (attributesNode.ChildNodes[i].Name == "Feature")
					{
						features.Add(attributesNode.ChildNodes[i].InnerText);
					}
				}
            return features;
		}

        /// <summary>
        /// Gets the image sets.
        /// </summary>
        /// <returns>The image sets.</returns>
        /// <param name="imageSets">Image sets.</param>
        public static List<ItemImage> GetImageSets(XmlNodeList imageSets){
            var images = new List<ItemImage>();
			if (imageSets.Count > 0)
			{
				for (int i = 0; i < imageSets.Count; i++)
				{
					var thumbnailNode = HtmlHelper.FindNode("ThumbnailImage", imageSets[i]);
					var thumbnailUrlNode = (thumbnailNode != null) ? HtmlHelper.FindNode("URL", thumbnailNode) : null;
					var largeImageNode = HtmlHelper.FindNode("LargeImage", imageSets[i]);
					var largeUrlImageNode = (largeImageNode != null) ? HtmlHelper.FindNode("URL", largeImageNode) : null;

					ItemImage img = new ItemImage();
					img.UrlThumbnail = thumbnailUrlNode.InnerText;
					img.UrlLargeImage = largeUrlImageNode.InnerText;

					images.Add(img);
				}
			}

            return images;
        }

		public static List<String> GetFeatures(XmlNodeList featureNodes)
		{
			var features = new List<String>();
			if (featureNodes.Count > 0)
			{
				for (int i = 0; i < featureNodes.Count; i++)
				{
                    var featureText = featureNodes[i].InnerText;
			
                    features.Add(featureText);
				}
			}

			return features;
		}

		/// <summary>
		/// Extract the string representaion of a html body from a http request with the given url.
		/// </summary>
		/// <returns>The html body.</returns>
		/// <param name="url">URL.</param>
        public static string GetWebPage(string url)
		{
			var result = "";
            url = DecodeUrlString(url);
			try
			{				
				using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
				{					
                    using(HttpResponseMessage response = client.GetAsync(url).Result){
                        response.EnsureSuccessStatusCode();
                        using(HttpContent content = response.Content){
                            result = content.ReadAsStringAsync().Result;
                        }
                    }               
				}				
			}
			catch (Exception e)
			{
				Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
			}
			return result;
		}



        /// <summary>
        /// Async operation of getting a web content.
        /// </summary>
        /// <returns>The get async.</returns>
        /// <param name="URI">URI.</param>
		public static async Task<string> HttpGetAsync(string URI)
		{
            StreamReader am = null;
			try
			{
				HttpClient hc = new HttpClient();
				Task<Stream> result = hc.GetStreamAsync(URI);

				Stream vs = await result;
				am = new StreamReader(vs);                				
			}
			catch (Exception ex)
			{
                Console.WriteLine("HtmlHelper.HttpGetAsync Error: " + ex.Message);
            }
            return await am.ReadToEndAsync();
		}

		/// <summary>
		/// Extracts the average star reviews from a text
		/// </summary>
		/// <returns>The star review from text. For ex., "4.5 out of 5 stars"</returns>
		public static String GetStarReview(String url, out float aveStars)
		{
			var result = "";
            aveStars = 0;
            var text = GetWebPage(url);
            Regex regex = new Regex(@"[0-9](\.[0-9])?\s(out of 5 stars)");
			Match match = regex.Match(text);
			if (match.Success)
			{
                result = match.Groups[0].Value;

                string label = match.Groups[0].Value;
                var reg = new Regex(@"[0-9](\.[0-9])?");
                var m = reg.Match(label);
                if(m.Success){
                    float.TryParse(m.Groups[0].Value, out aveStars);
                }
                result = label;

            }
			return result;
		}

		public static string DecodeUrlString(string url)
		{
			string newUrl;
			while ((newUrl = Uri.UnescapeDataString(url)) != url)
				url = newUrl;
			return newUrl;
		}

        public static void SendMail(){
            
        }

    }

}
