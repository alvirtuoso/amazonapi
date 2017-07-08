using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using AmazonOperations;
using AmazonOperations.Model;
using System.Threading.Tasks;

[Route("api/[controller]")]
public class SearchController : Controller
{
    private const string ASSOCIATE_ID = "alvirtuoso-20";

    private AmazonAuthentication GetConfig()
    {
        var accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

        var authentication = new AmazonAuthentication();
        authentication.AccessKey = accessKey;
        authentication.SecretKey = secretKey;

        return authentication;
    }

    // GET api/search/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value test Get ID";
    //}

    // api/search/{key}
   [HttpGet("{key}")]
    public ActionResult Search(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            //return View();
            return null;
        }


        var authentication = this.GetConfig();

        var wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, ASSOCIATE_ID);
        var responseGroup = AmazonResponseGroup.ItemAttributes | AmazonResponseGroup.Images;

        var result = wrapper.Search(key.Trim(), AmazonSearchIndex.All, responseGroup);

        //return View(result);
        return new JsonResult(result);
    }

}
