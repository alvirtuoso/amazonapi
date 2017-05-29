using System;
namespace AmazonProductSearch.Models
{
  
	public class CartItem
	{
		public string Asin { get; set; }
		public int Quantity { get; set; }

		public CartItem()
		{ }

		public CartItem(string asin, int quantity = 1)
		{
			this.Asin = asin;
			this.Quantity = quantity;
		}
	}

}
