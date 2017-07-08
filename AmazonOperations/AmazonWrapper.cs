﻿using AmazonOperations.Model;
using AmazonOperations.Operation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace AmazonOperations
{
    public partial class AmazonWrapper : IAmazonWrapper
    {
        private AmazonAuthentication _authentication;
        private AmazonEndpoint _endpoint;
        private string _associateTag;
        private string _userAgent = null;

        //public event Action<string> XmlReceived;
        public event Action<AmazonErrorResponse> ErrorReceived;

        public AmazonWrapper(AmazonAuthentication authentication, AmazonEndpoint endpoint, string associateTag = "nagerat-21")
        {
            this._authentication = authentication;
            this._endpoint = endpoint;
            this._associateTag = associateTag;
        }

        public void SetEndpoint(AmazonEndpoint amazonEndpoint)
        {
            this._endpoint = amazonEndpoint;
        }

        public void SetUserAgent(string userAgent)
        {  
            this._userAgent = userAgent;  
        }

        public AmazonItemLookupOperation ItemLookupOperation(IList<string> articleNumbers, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.Large)
        {
            var operation = new AmazonItemLookupOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.Get(articleNumbers);
            operation.AssociateTag(this._associateTag);

            return operation;
        }

        public AmazonItemSearchOperation ItemSearchOperation(string search, AmazonSearchIndex amazonSearchIndex = AmazonSearchIndex.All, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.Large)
        {
            var operation = new AmazonItemSearchOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.Keywords(search);
            operation.SearchIndex(amazonSearchIndex);
            operation.AssociateTag(this._associateTag);
            return operation;
        }

        public AmazonBrowseNodeLookupOperation BrowseNodeLookupOperation(long browseNodeId, AmazonResponseGroup amazonResponseGroup = AmazonResponseGroup.BrowseNodeInfo)
        {
            var operation = new AmazonBrowseNodeLookupOperation();
            operation.ResponseGroup(amazonResponseGroup);
            operation.BrowseNodeId(browseNodeId);
            operation.AssociateTag(this._associateTag);

            return operation;
        }

        public AmazonCartCreateOperation CartCreateOperation(IList<AmazonCartItem> amazonCartItems)
        {
            var operation = new AmazonCartCreateOperation();
            operation.AssociateTag(this._associateTag);
            operation.AddArticles(amazonCartItems);
            return operation;
        }

        /// <summary>
        /// ItemLookup
        /// </summary>
        /// <param name="articleNumber">ASIN, EAN, GTIN, ISBN</param>
        /// <param name="responseGroup"></param>
        /// <returns></returns>
        public AmazonItemResponse Lookup(string articleNumber, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            return this.Lookup(new string[1] { articleNumber }, responseGroup);
        }

        public AmazonItemResponse Lookup(IList<string> articleNumbers, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemLookupOperation(articleNumbers, responseGroup);

            var webResponse = this.Request(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<ItemLookupResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<ItemLookupErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public AmazonItemResponse Search(string search, AmazonSearchIndex searchIndex = AmazonSearchIndex.All, AmazonResponseGroup responseGroup = AmazonResponseGroup.Large)
        {
            var operation = this.ItemSearchOperation(search, searchIndex, responseGroup);

            var webResponse = this.Request(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<ItemSearchResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<ItemSearchErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public BrowseNodeLookupResponse BrowseNodeLookup(long browseNodeId, AmazonResponseGroup responseGroup = AmazonResponseGroup.BrowseNodeInfo)
        {
            var operation = this.BrowseNodeLookupOperation(browseNodeId, responseGroup);

            var webResponse = this.Request(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<BrowseNodeLookupResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<BrowseNodeLookupErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public CartCreateResponse CartCreate(IList<AmazonCartItem> amazonCartItems)
        {
            var operation = this.CartCreateOperation(amazonCartItems);

            var webResponse = this.Request(operation);
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                return XmlHelper.ParseXml<CartCreateResponse>(webResponse.Content);
            }
            else
            {
                var errorResponse = XmlHelper.ParseXml<CartCreateErrorResponse>(webResponse.Content);
                this.ErrorReceived?.Invoke(errorResponse);
            }

            return null;
        }

        public ExtendedWebResponse Request(AmazonOperationBase amazonOperation)
        {
            using (var amazonSign = new AmazonSign(this._authentication, this._endpoint))
            {
                var requestUri = amazonSign.Sign(amazonOperation);
                return SendRequest(requestUri);
            }
        }


        public async Task<string> GetAsync(string uri)
		{
			var httpClient = new HttpClient();
			var content = await httpClient.GetStringAsync(uri);
			return content;

		}

        private ExtendedWebResponse SendRequest(string uri)
        {
            var result = "";
            try
            {
				using (var client = new HttpClient())
				{
					using (HttpResponseMessage response = client.GetAsync(uri).Result)
					{
						response.EnsureSuccessStatusCode();
                       
						using (HttpContent content = response.Content)
						{
                            result = content.ReadAsStringAsync().Result;
						}
					}
				}
				

                return new ExtendedWebResponse(HttpStatusCode.OK, result);
                    

            }      
            catch (Exception exception)
            {
                return new ExtendedWebResponse(HttpStatusCode.SeeOther, exception.Message);
            }
        }
    }
}