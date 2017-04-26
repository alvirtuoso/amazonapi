using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using AmazonProductSearch.Models;
using AmazonProductSearch.Helpers;
using System.Threading.Tasks;

namespace AmazonProductSearch.Controllers
{
    [Route("api/[controller]")]
    public class AmazonController : Controller
    {
        private const string MY_AWS_ACCESS_KEY_ID = "AKIAJHBDJY4JNUZAPLPQ";
        private const string MY_AWS_SECRET_KEY = "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";
        private const string MY_ASSOCIATE_ID = "alvirtuoso-20";
        private const string DESTINATION = "webservices.amazon.com"; //"ecs.amazonaws.com";

        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/";
        private const string ITEM_ID = "B00URGA106"; // testing 

        SignedRequestHelper signHelper = new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION);

        // GET api/amazon?search=
        [HttpGet]
        public ActionResult Get(String search)
        {            
            var items = new List<Item>();
            var result = new JsonResult("");

			try
            {
                var url = signHelper.Sign(StoreInDictionary(search));
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponseAsync().Result;

                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());
                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message");
                if (!(errorMessageNodes != null && errorMessageNodes.Count > 0))
                {
                    XmlNodeList itemList = doc.GetElementsByTagName("Item");
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        Item item = new Item();
                        // Get the <ItemAttributes> node of an <Item>                               <Item>
                        var attributeNode = HtmlHelper.FindNode("ItemAttributes", itemList[i]);  //  <ItemAttributes>
                        var titleNode = HtmlHelper.FindNode("Title", attributeNode);             //       <Title>

                        // Some items does not have listed price.(ex. item with EAN# 0886424573142) 
                        var priceNode = HtmlHelper.FindNode("ListPrice", attributeNode);           //     <ListPrice>
                       
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
                        item.Title = titleNode.InnerText;                                                                        // </ReviewsNode>
                        item.Price = (formattedPriceNode != null) ? formattedPriceNode.InnerText : string.Empty;
                        item.UrlMediumImage = (medImgUrlNode != null) ? medImgUrlNode.InnerText : string.Empty;
                        item.UrlReview = (reviewUrlNode != null) ? reviewUrlNode.InnerText : string.Empty;
                        item.UrlLargeImage = (lgImgUrlNode != null) ? lgImgUrlNode.InnerText : string.Empty;
                        // item.StarLabel = HtmlHelper.GetStarReview(item.UrlReview, out aveStars);
                        item.AverageStars = aveStars;
                        items.Add(item);
    
                    }
                    result = new JsonResult(items);
                }
                else
                {
                    result = new JsonResult("Error: " + errorMessageNodes.Item(0).InnerText);
                }

            }
            catch (Exception e)
            {
                result = new JsonResult("server error");
            }

            return result;
        }

        private static IDictionary<string, string> StoreInDictionary(string searchText)
        {
            IDictionary<string, string> r1 = new Dictionary<string, String>();

            r1["Service"] = "AWSECommerceService";
            r1["Operation"] = "ItemSearch";
            r1["AssociateTag"] = MY_ASSOCIATE_ID;
            r1["SearchIndex"] = "All";
            r1["ResponseGroup"] = "Images,ItemAttributes,OfferListings,Offers,Reviews";
            r1["Keywords"] = searchText;  
            r1["IncludeReviewsSummary"] = "true";
            return r1;
        }

        // GET api/amazon/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

  
    }
}
