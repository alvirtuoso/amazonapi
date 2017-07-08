using System;
namespace AmazonProductSearch.Operation
{
    public class SearchItemOperation: OperationBase
    {
        public SearchItemOperation()
        {
			base.StoreInDictionary.Add("Operation", "ItemLookup");
        }

		public void PresetOperation(string asin)
		{
			this.AddService("AWSECommerceService");
			this.AddAssociateTag("alvirtuoso-20");
			this.AddOrReplace("ResponseGroup", "Images,ItemAttributes,Reviews,Offers,SalesRank,Similarities");

            this.AddOrReplace("ItemId", asin);
            this.AddOrReplace("IdType", "ASIN");

		}
    }
}
