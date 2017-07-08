using System;
using System.Collections.Generic;
using AmazonProductSearch.Models;

namespace AmazonProductSearch.Operation
{
    public class CartOperation : OperationBase
    {
        public CartOperation()
        {
            // Initialize parameters for Dictionary object
            base.StoreInDictionary.Add("Operation", "CartCreate");
			this.AddService("AWSECommerceService");
			//this.AddAssociateTag("alvirtuoso-20");
        }

        /// <summary>
        /// Creates an amazon remote cart then adds items.
        /// </summary>
        /// <param name="cartItems">Cart items.</param>
        public Dictionary<string, string> AddCartItems(IList<AmzCartItem> cartItems, string associateId, bool includeTopSellers)
        {
            this.AddAssociateTag(associateId);
            var i = 0;
            foreach (var item in cartItems)
            {
                base.StoreInDictionary.Add($"Item.{i}.OfferListingId", item.OfferListingId);
                base.StoreInDictionary.Add($"Item.{i}.Quantity", item.Quantity.ToString());
                if(includeTopSellers){
					base.StoreInDictionary.Add("ResponseGroup", "CartTopSellers");
                }
                i++;
            }
            return base.StoreInDictionary;
        }


    }

}
