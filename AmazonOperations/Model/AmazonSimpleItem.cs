﻿namespace AmazonOperations.Model
{
    public class AmazonSimpleItem
    {
        public string Asin { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string[] Description { get; set; }
        public Item SourceItem { get; set; }

        public AmazonSimpleItem(Item item)
        {
            this.SourceItem = item;
            this.Asin = item.ASIN;

            if (item.ItemAttributes != null)
            {
                this.Name = item.ItemAttributes.Title;
                this.Description = item.ItemAttributes.Feature;
            }
            
            if (item.LargeImage != null)
            {
                this.ImageUrl = item.LargeImage.URL;
            }

            if (item.Offers != null)
            {
                if (item.Offers.Offer != null && item.Offers.Offer.Length > 0)
                {
                    if (item.Offers.Offer[0].OfferListing != null && item.Offers.Offer[0].OfferListing.Length > 0)
                    {
                        double price;
                        var amount = item.Offers.Offer[0].OfferListing[0].Price.Amount;
                        if (double.TryParse(amount, out price))
                        {
                            this.Price = price / 100;
                        }
                    }
                }
            }
        }

        public string GetDetailPageUrl(AmazonEndpoint amazonEndpoint, string associateTag = "nagerat-21")
        {
            var domain = AmazonDomain.GetDomain(amazonEndpoint);

            return $"https://www.{domain}/dp/{Asin}?tag={associateTag}";
        }
    }
}