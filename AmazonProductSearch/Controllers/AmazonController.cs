using System;
using System.Collections.Generic;
using System.Net;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using AmazonProductSearch.Models;
using AmazonProductSearch.Helpers;
//using Nager.AmazonProductAdvertising;

namespace AmazonProductSearch.Controllers
{
    [Route("api/[controller]")]
    public class AmazonController : Controller
    {
		private const string MY_AWS_ACCESS_KEY_ID = "";
		private const string MY_AWS_SECRET_KEY = "";
		private const string MY_ASSOCIATE_ID = "";

        private const string DESTINATION = "webservices.amazon.com"; //"ecs.amazonaws.com";
        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/";
        private const string ITEM_ID = "B00URGA106"; // testing 
        private string AKEY = Environment.GetEnvironmentVariable("ACCESS_KEY");
        private string SKEY = Environment.GetEnvironmentVariable("SECRET_KEY");

        SignedRequestHelper signHelper = new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION);


        private const string AssociateTag = "alvirtuoso-20";
		//private AmazonAuthentication GetConfig()
		//{
		//	var accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY");
		//	var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

		//	var authentication = new AmazonAuthentication();
		//	authentication.AccessKey = accessKey;
		//	authentication.SecretKey = secretKey;

		//	return authentication;
		//}

        /// <summary>
        /// Get request for Averages the stars. api/amazon/reviews/stringdata
        /// </summary>
        /// <returns>The stars. For example "4.5 out of 5 stars". Also returns average stars in float format.</returns>
        /// <param name="review">Url of the page Review.</param>
        [HttpGet("reviews/{review}")]
        public ActionResult AverageStars(String review){
            var result = new JsonResult("");
            float aveStars = 0;
            try
            {
                result.Value = HtmlHelper.GetStarReview(review, out aveStars);
            }
            catch (Exception)
            {
                // TODO: Add logging.
                result.Value = "Server Error";
            }
            //var n = new Nager.AmazonProductAdvertising.Operation.AmazonItemSearchOperation().
            return result;
        }

  //      [HttpGet("search/{key}")]
		//public ActionResult Search(string key)
		//{
		//	if (string.IsNullOrEmpty(key))
		//	{
		//		//return View();
		//		return null;
		//	}


		//	var authentication = this.GetConfig();

		//	var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, AssociateTag);
		//	var responseGroup = Nager.AmazonProductAdvertising.Model.AmazonResponseGroup.ItemAttributes | Nager.AmazonProductAdvertising.Model.AmazonResponseGroup.Images;

		//	var result = wrapper.Search(key.Trim(), Nager.AmazonProductAdvertising.Model.AmazonSearchIndex.All, responseGroup);

		//	//return View(result);
		//	return new JsonResult(result);
		//}

		///<summary>
		/// GET api/amazon/items/stringsearchword/2 
		/// </summary>
        ///  <param name="search">search</param> 
		/// <param name="page">page</param>
		[HttpGet("items/{search}/{page}")]
        public ActionResult GetItems(String search, String page)    // [HttpGet] will have api/amazon?search=usersearchword&page=2
        {            
            var items = new List<Item>();
            var result = new JsonResult("");

			try
            {
                var url = signHelper.Sign(StoreInDictionary(search, page));
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponseAsync().Result;

                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());
                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
                if (!(errorMessageNodes != null && errorMessageNodes.Count > 0))
                {
                     XmlNodeList itemList = doc.GetElementsByTagName("Item");
                    var isPrimeEligibleNode = doc.GetElementsByTagName("IsEligibleForPrime");
                   for (int i = 0; i < itemList.Count; i++)
                    {
                        Item item = new Item();

                        // Get the offers xml tag
                        var offersNode = HtmlHelper.FindNode("Offers", itemList[i]);

                        // Get the <ItemAttributes> node of an <Item>                               <Item>
                        var attributeNode = HtmlHelper.FindNode("ItemAttributes", itemList[i]);  //  <ItemAttributes>
                        var titleNode = HtmlHelper.FindNode("Title", attributeNode);             //       <Title>

                        // Some items does not have listed price.(ex. item with EAN# 0886424573142)
                        // ** List Price is not always the displayed one, but the price under <Offers> xml tag. **
                        var priceNode = HtmlHelper.FindNode("ListPrice", attributeNode);           //     <ListPrice>
                        var itemLinkNode = HtmlHelper.FindNode("DetailPageURL", itemList[i]);
                        var asinNode = HtmlHelper.FindNode("ASIN", itemList[i]);       
                        // Don't include item with no price
                        if(null == priceNode){
                            continue;
                        }
                        float aveStars = 0;
                        var formattedPriceNode = (priceNode != null)? HtmlHelper.FindNode("FormattedPrice", priceNode): null;    // <FormattedPrice>
                        var medImgNode = HtmlHelper.FindNode("MediumImage", itemList[i]);                                  //       <MediumImage>
                        var medImgUrlNode = (medImgNode != null) ? HtmlHelper.FindNode("URL", medImgNode): null;                      //<URL>https://someurel/src.jpg</URL>
                        var lgImgNode = HtmlHelper.FindNode("LargeImage", itemList[i]);           //                                <LargeImage>
                        var lgImgUrlNode = (lgImgNode != null) ? HtmlHelper.FindNode("URL", lgImgNode): null;                        // <URL> someurl </URL>
                        var reviewsNode = HtmlHelper.FindNode("CustomerReviews", itemList[i]);                                 //   <ReviewsNode>
                        var reviewUrlNode = (reviewsNode != null) ? HtmlHelper.FindNode("IFrameURL", reviewsNode) : null;       //  <IFrameUrl></IFrameURL/>

                        var currencySign = "";
						item.Title = titleNode.InnerText;                                                                       
						item.DisplayedPrice = HtmlHelper.FindDisplayedPrice(offersNode);
                        item.Price = HtmlHelper.FindPrice(formattedPriceNode, out currencySign);
                        item.CurrencySign = currencySign;
                        item.UrlItemLink = (itemLinkNode != null) ? itemLinkNode.InnerText : string.Empty;
                        item.UrlMediumImage = (medImgUrlNode != null) ? medImgUrlNode.InnerText : string.Empty;
                        item.UrlReview = (reviewUrlNode != null) ? reviewUrlNode.InnerText : string.Empty;

                        item.StarLabel = "";// HtmlHelper.GetStarReview(item.UrlReview, out aveStars);
                        item.AverageStars = aveStars;
                        item.ID = Guid.NewGuid();
                        item.Asin = (asinNode != null) ? asinNode.InnerText : string.Empty;
                        item.IsPrimeEligible = HtmlHelper.IsPrimeEligible(isPrimeEligibleNode);
                        item.Features = HtmlHelper.ItemFeatures(attributeNode);

                        // Get Large Image
                        var imgSetsNode = HtmlHelper.FindNode("ImageSets", itemList[i]);
                        item.UrlLargeImage = (imgSetsNode != null) ? HtmlHelper.GetLargeImgLink(imgSetsNode.ChildNodes) : string.Empty;

                        items.Add(item);
    
                    }
                    result = new JsonResult(items);
                }
                else
                {
                    result = new JsonResult("Error: " + errorMessageNodes.Item(0).InnerText);
                }

            }
            catch (Exception ex)
            {
                var m = ex.Message;
				// TODO: Add logging.
				result = new JsonResult("server error");
            }

            return result;
        }

		///<summary>
		/// GET api/amazon/items/stringsearchword/2 
		/// </summary>
		///  <param name="asin">asin</param> 
		[HttpGet("item/{asin}")]
		public ActionResult GetItem(String asin)    // [HttpGet] will have api/amazon?search=usersearchword&page=2
		{
			var items = new Item();
			var result = new JsonResult("");

			try
			{
				var url = signHelper.Sign(StoreInDictionary(asin));
				WebRequest request = WebRequest.Create(url);
				WebResponse response = request.GetResponseAsync().Result;

				XmlDocument doc = new XmlDocument();
				doc.Load(response.GetResponseStream());
				XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
				if (!(errorMessageNodes != null && errorMessageNodes.Count > 0))
				{
                    var itemNodeList = doc.GetElementsByTagName("Item");
                    var imageSets = doc.GetElementsByTagName("ImageSet");
                    var features = doc.GetElementsByTagName("Feature");
                    var isPrimeEligibleNode = doc.GetElementsByTagName("IsEligibleForPrime");
                    if (itemNodeList.Count > 0)
                    {
                        var itemNode = itemNodeList[0];
                        Item item = new Item();

						// Get the <Offers> node
                        var offersNode = HtmlHelper.FindNode("Offers", itemNode);

                        // Get the <ItemAttributes> node of an <Item>                              
                        var attributeNode = HtmlHelper.FindNode("ItemAttributes", itemNode);  //  <ItemAttributes>
                        var titleNode = HtmlHelper.FindNode("Title", attributeNode);             //       <Title>

                        // Some items does not have listed price.(ex. item with EAN# 0886424573142) 
                        var priceNode = HtmlHelper.FindNode("ListPrice", attributeNode);           //     <ListPrice>
                        var itemLinkNode = HtmlHelper.FindNode("DetailPageURL", itemNode);
                        var asinNode = HtmlHelper.FindNode("ASIN", itemNode);
						
						
                        float aveStars = 0;
                        var formattedPriceNode = (priceNode != null) ? HtmlHelper.FindNode("FormattedPrice", priceNode) : null;    // <FormattedPrice>
                        var smallImgNode = HtmlHelper.FindNode("SmallImage", itemNode);
                        var smallImgUrlNode = (smallImgNode != null) ? HtmlHelper.FindNode("URL", smallImgNode) : null;
						var medImgNode = HtmlHelper.FindNode("MediumImage", itemNode);                                  //       <MediumImage>
                        var medImgUrlNode = (medImgNode != null) ? HtmlHelper.FindNode("URL", medImgNode) : null;                      //<URL>https://someurel/src.jpg</URL>
                        var lgImgNode = HtmlHelper.FindNode("LargeImage", itemNode);           //                                <LargeImage>
                        var lgImgUrlNode = (lgImgNode != null) ? HtmlHelper.FindNode("URL", lgImgNode) : null;                        // <URL> someurl </URL>
                        var reviewsNode = HtmlHelper.FindNode("CustomerReviews", itemNode);                                 //   <ReviewsNode>
                        var reviewUrlNode = (reviewsNode != null) ? HtmlHelper.FindNode("IFrameURL", reviewsNode) : null;       //  <IFrameUrl></IFrameURL/>

                        var currencySign = "";

                        item.Title = titleNode.InnerText;                                                                        // </ReviewsNode>
						item.DisplayedPrice = HtmlHelper.FindDisplayedPrice(offersNode);
                        item.Price = HtmlHelper.FindPrice(formattedPriceNode, out currencySign);
                        item.CurrencySign = currencySign;
                        item.UrlItemLink = (itemLinkNode != null) ? itemLinkNode.InnerText : string.Empty;
                        item.UrlSmallImage = (smallImgUrlNode != null) ? smallImgUrlNode.InnerText : string.Empty;
                        item.UrlMediumImage = (medImgUrlNode != null) ? medImgUrlNode.InnerText : string.Empty;
                        item.UrlReview = (reviewUrlNode != null) ? reviewUrlNode.InnerText : string.Empty;
                        item.UrlLargeImage = (lgImgUrlNode != null) ? lgImgUrlNode.InnerText : string.Empty;
                        item.StarLabel = "";// HtmlHelper.GetStarReview(item.UrlReview, out aveStars);
                        item.AverageStars = aveStars;
                        item.ID = Guid.NewGuid();
                        item.Asin = (asinNode != null) ? asinNode.InnerText : string.Empty;
                        item.IsPrimeEligible = HtmlHelper.IsPrimeEligible(isPrimeEligibleNode);
                        item.ItemImages = HtmlHelper.GetImageSets(imageSets);
                        item.Features = HtmlHelper.GetFeatures(features);

                        result = new JsonResult(item);
                    }
				}
				else
				{
					result = new JsonResult("Error: " + errorMessageNodes.Item(0).InnerText);
				}

			}
			catch (Exception ex)
			{
				var m = ex.Message;
				// TODO: Add logging.
				result = new JsonResult("server error");
			}

			return result;
		}

        /// <summary>
        /// Stores the aws params in dictionary for url signing
        /// </summary>
        /// <returns>The in dictionary.</returns>
        /// <param name="searchText">Search text.</param>
        /// <param name="page">Page.</param>
        private static IDictionary<string, string> StoreInDictionary(string searchText, string page)
        {
            IDictionary<string, string> r1 = new Dictionary<string, String>();

            r1["Service"] = "AWSECommerceService";
            r1["Operation"] = "ItemSearch";
            r1["AssociateTag"] = MY_ASSOCIATE_ID;
            r1["SearchIndex"] = "All";
            r1["ResponseGroup"] = "Images,ItemAttributes,Reviews,Offers,SalesRank";
            r1["Keywords"] = searchText;  
            r1["ItemPage"] = page;
            //r1["IncludeReviewsSummary"] = "true";
            return r1;
        }

        /// <summary>
        /// Stores the in dictionary.
        /// </summary>
        /// <returns>The in dictionary.</returns>
        /// <param name="asin">ASIN.</param>
		private static IDictionary<string, string> StoreInDictionary(string asin)
		{
			IDictionary<string, string> r1 = new Dictionary<string, String>();

			r1["Service"] = "AWSECommerceService";
			r1["Operation"] = "ItemLookup";
			r1["AssociateTag"] = MY_ASSOCIATE_ID;
			r1["ItemId"] = asin;
			r1["ResponseGroup"] = "Images,ItemAttributes,Reviews,Offers,SalesRank,Similarities";
			r1["IdType"] = "ASIN";
	
			return r1;
		}

        // GET api/amazon/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value test Get ID";
        //}

  
    }
}
