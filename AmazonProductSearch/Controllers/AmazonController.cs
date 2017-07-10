﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using AmazonProductSearch.Models;
using AmazonProductSearch.Helpers;
using AmazonProductSearch.Operation;

[Route("api/[controller]")]
public class AmazonController : Controller
{
    private const string ASSOCIATE_ID = "alvirtuoso-20";

    //private static string endPoint = "webservices.amazon.com"; //"ecs.amazonaws.com"; $"webservices.
    private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/";
    private const string ITEM_ID = "B00URGA106"; // testing 

    //SignedRequestHelper signHelper = new SignedRequestHelper(
    //Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), endPoint);
    SignedRequestHelper signHelper = new SignedRequestHelper();

    /// <summary>
    /// Get request for Averages the stars. api/amazon/reviews/stringdata
    /// </summary>
    /// <returns>The stars. For example "4.5 out of 5 stars". Also returns average stars in float format.</returns>
    /// <param name="review">Url of the page Review.</param>
    [HttpGet("reviews/{review}")]
    public ActionResult AverageStars(String review)
    {
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

        return result;
    }

    //// POST: api/item/createforcard items
    //[HttpPost("createforcard")]
    //public IEnumerable<Item> CreateForCard([FromBody]Item item)
    //{
    //    IEnumerable<Item> items = new List<Item>();
    //    if (ModelState.IsValid)
    //    {
    //        items = this.itemRepo.AddItemToCard(item);
    //        //return RedirectToAction("Index");
    //    }

    //    return items;
    //}

    /// <summary>
    /// Adds items to cart. api/amazon/cart/{cartItems}
    /// </summary>
    /// <returns>The to cart.</returns>
    [HttpPost("cart")]
    public ActionResult AddToCart([FromBody]Checkout checkout)//([FromBody]List<AmzCartItem> items, string id)
    {
        var result = string.Empty;
        var id = (checkout.Id == "undefined" || checkout.Id == string.Empty) ? ASSOCIATE_ID : checkout.Id;
        CartOperation cartOp = new CartOperation();
        Cart cart = new Cart();
        try
        {
            var d = cartOp.AddCartItems(checkout.items, id, false);
            //var url = signHelper.Sign(d);
            string url = "";
            string endPoint = HtmlHelper.EndPointUrl();
            using (SignedRequestHelper signer = new SignedRequestHelper(
                Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), endPoint))
            {

                url = signer.Sign(d); //signHelper.Sign(StoreInDictionary(search, page));
            }
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponseAsync().Result;

            XmlDocument doc = new XmlDocument();

            doc.Load(response.GetResponseStream());
            var reqNodes = doc.GetElementsByTagName("Request");
            var cidNodes = doc.GetElementsByTagName("CartId");
            var purchaseUrlNodes = doc.GetElementsByTagName("PurchaseURL");
            Request req = new Request();
            req.IsValid = bool.Parse(reqNodes[0].FirstChild.InnerText);
            cart.CartId = cidNodes[0].InnerText;
            cart.Request = req;
            cart.PurchaseURL = purchaseUrlNodes[0].InnerText;

        }
        catch (Exception ex)
        {
            // TODO: create a logger
            result = "Server Error: " + ex.Message;
        }
        return new JsonResult(cart);
    }


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
            string url = "";
            string endPoint = HtmlHelper.EndPointUrl();
            using (SignedRequestHelper signer = new SignedRequestHelper(
                Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), endPoint)){

                url = signer.Sign(StoreInDictionary(search, page)); //signHelper.Sign(StoreInDictionary(search, page));
            }

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
                    if (null == priceNode)
                    {
                        continue;
                    }
                    float aveStars = 0;
                    var formattedPriceNode = (priceNode != null) ? HtmlHelper.FindNode("FormattedPrice", priceNode) : null;    // <FormattedPrice>
                    var medImgNode = HtmlHelper.FindNode("MediumImage", itemList[i]);                                  //       <MediumImage>
                    var medImgUrlNode = (medImgNode != null) ? HtmlHelper.FindNode("URL", medImgNode) : null;                      //<URL>https://someurel/src.jpg</URL>
                    var lgImgNode = HtmlHelper.FindNode("LargeImage", itemList[i]);           //                                <LargeImage>
                    var lgImgUrlNode = (lgImgNode != null) ? HtmlHelper.FindNode("URL", lgImgNode) : null;                        // <URL> someurl </URL>
                    var reviewsNode = HtmlHelper.FindNode("CustomerReviews", itemList[i]);                                 //   <ReviewsNode>
                    var reviewUrlNode = (reviewsNode != null) ? HtmlHelper.FindNode("IFrameURL", reviewsNode) : null;       //  <IFrameUrl></IFrameURL/>
                    var offersSummary = HtmlHelper.FindNode("OfferSummary", itemList[i]);

                    var currencySign = "";
                    item.Title = titleNode.InnerText;
                    item.OfferListingId = HtmlHelper.FindOfferListingID(offersNode);
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
                    item.LowestPrice = HtmlHelper.LowestPrice(offersSummary);

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
            result = new JsonResult(m);
        }

        return result;
    }

    [HttpGet("{id}")]
    public ActionResult SearchByAmazonID(String id){
        var result = new JsonResult("");


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
            string url = "";
            string endPoint = HtmlHelper.EndPointUrl();
            using (SignedRequestHelper signer = new SignedRequestHelper(
                Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), endPoint))
            {
                url = signer.Sign(StoreInDictionary(asin));
            }
            //var url = signHelper.Sign(StoreInDictionary(asin));
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
                    var offersSummary = HtmlHelper.FindNode("OfferSummary", itemNode);
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
                    item.OfferListingId = HtmlHelper.FindOfferListingID(offersNode);
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
                    item.LowestPrice = HtmlHelper.LowestPrice(offersSummary);

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
            //result = new JsonResult("server error");
            throw new Exception(ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Stores the aws params in dictionary for url signing. For "search all" operation.
    /// </summary>
    /// <returns>The in dictionary.</returns>
    /// <param name="searchText">Search text.</param>
    /// <param name="page">Page.</param>
    private static IDictionary<string, string> StoreInDictionary(string searchText, string page)
    {
        SearchOperation searchOp = new SearchOperation();
        searchOp.PresetOperation(searchText, page);

        return searchOp.StoreInDictionary;
  
    }

    /// <summary>
    /// Stores the in dictionary. For specific item search operation
    /// </summary>
    /// <returns>The in dictionary.</returns>
    /// <param name="asin">ASIN.</param>
    private static IDictionary<string, string> StoreInDictionary(string asin)
    {
        SearchItemOperation searchOp = new SearchItemOperation();
        searchOp.PresetOperation(asin);
        return searchOp.StoreInDictionary;

    }

 




}
