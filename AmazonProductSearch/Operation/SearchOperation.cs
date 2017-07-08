using System;
namespace AmazonProductSearch.Operation
{
    public class SearchOperation: OperationBase
    {
        public SearchOperation()
        {
            this.AddOrReplace("Operation", "ItemSearch");

        }

        /// <summary>
        /// Sets Service, AssociateTag, SearchIndex, ResponseGroup, Keywords, and ItemPage
        /// </summary>
        public void PresetOperation(string keywords, string page){
            this.AddService("AWSECommerceService");
            this.AddAssociateTag("alvirtuoso-20");
            this.AddOrReplace("SearchIndex", "All");
            this.AddOrReplace("ResponseGroup", "Images,ItemAttributes,Reviews,Offers,SalesRank");
            this.AddOrReplace("Keywords", keywords);
            this.AddOrReplace("ItemPage", page);
        }

    }
}
