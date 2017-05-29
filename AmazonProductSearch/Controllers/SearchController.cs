using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AmazonProductSearch.Controllers
{
    public class SearchController
    {
        public SearchController()
        {
        }

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

  //      /// <summary>
  //      /// Lookup the specified articleNumber.
  //      /// </summary>
  //      /// <returns>The lookup.</returns>
  //      /// <param name="articleNumber">Article number.</param>
  //      [HttpGet("")]
		//public ActionResult Lookup(string articleNumber)
		//{
		//	if (string.IsNullOrEmpty(articleNumber))
		//	{
		//		return null;
		//	}

		//	var authentication = this.GetConfig();

		//	var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, AssociateTag);
		//	var result = wrapper.Lookup(articleNumber.Trim());

  //          return new JsonResult(result);
		//}

		//public ActionResult Lookups(string[] articleNumbers)
		//{
		//	if (articleNumbers == null)
		//	{
		//		return null;
		//	}

		//	var authentication = this.GetConfig();

		//	var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, AssociateTag);
		//	var result = wrapper.Lookup(articleNumbers);

  //          //return View("Lookup", result);
  //          return new JsonResult(result);
		//}

		//public ActionResult Search(string search)
		//{
		//	if (string.IsNullOrEmpty(search))
		//	{
  //              //return View();
  //              return null;
		//	}


		//	var authentication = this.GetConfig();

		//	var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, AssociateTag);
		//	var responseGroup = AmazonResponseGroup.ItemAttributes | AmazonResponseGroup.Images;

		//	var result = wrapper.Search(search.Trim(), AmazonSearchIndex.All, responseGroup);

  //          //return View(result);
  //          return new JsonResult(result);
		//}

		//public ActionResult Detail(string articleNumber)
		//{
		//	if (string.IsNullOrEmpty(articleNumber))
		//	{
  //              //return View();
  //              return null;
  //          }

		//	var authentication = this.GetConfig();

		//	var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, AssociateTag);
		//	var result = wrapper.Lookup(articleNumber.Trim());

		//	var item = result.Items?.Item.FirstOrDefault();

  //          return new JsonResult(item);
		//	//return View(item);
		//}
    }
}
