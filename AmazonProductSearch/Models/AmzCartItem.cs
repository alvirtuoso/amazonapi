using System;
namespace AmazonProductSearch.Models
{
  
	public class AmzCartItem
	{
        private string offerListingId;
        private int quantity;
        //Item item;
        
		public AmzCartItem()
		{ }
		
        public AmzCartItem(string OfferListingId, int quantity = 1)
        {
            this.offerListingId = OfferListingId;
            this.quantity = quantity;
        }

        public string OfferListingId
        {
            get { return this.offerListingId; }
            set { this.offerListingId = value; }
        }

        public int Quantity { 
            get{ return this.quantity; }
            set{ this.quantity = value; }
        }

        //public Item Item
        //{
        //    get { return this.item; }
        //    set{ this.item = value; }
        //}

	}

}
