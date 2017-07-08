using System;
using System.Collections.Generic;

namespace AmazonProductSearch.Models
{
    /// <summary>
    /// Item.
    /// </summary>
    public class Item
    {
        private Guid id;
        private string asin;
        private string offerListingId;
        private string title;
        private float price;
        private float displayedPrice;
        private string currencySign;
        private string urlItemLink;
        private string urlReview;
        private string urlSmallImage;
        private string urlMediumImage;
        private string urlLargeImage;
        private string starLabel;
        private float averageStars;
        private float lowestPrice;
        private bool isPrimeEligible;
        private List<ItemImage> itemImages;
        private List<String> features;

        public Item(){}
        public Item(Guid Id, string asin,string offerListingId, string title, float price, float lowestPrice, float displayedPrice, string currencySign, string UrlItemLink, string urlReview, string urlSmallImage, string urlMediumImage, string urlLargeImage, string stars, bool isPrimeEligible, List<ItemImage> itemImages, List<String> features)
        {
            this.id = Id;
            this.asin = asin;
            this.offerListingId = offerListingId;
            this.title = title;
            this.price = price;
            this.lowestPrice = lowestPrice;
            this.displayedPrice = displayedPrice;
            this.currencySign = currencySign;
            this.urlItemLink = UrlItemLink;
            this.urlReview = urlReview;
            this.urlSmallImage = urlSmallImage;
            this.urlMediumImage = urlMediumImage;
            this.urlLargeImage = urlLargeImage;
            this.starLabel = stars;
            this.isPrimeEligible = isPrimeEligible;
            this.itemImages = itemImages;
            this.features = features;

        }

        public Guid ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public String OfferListingId{
            get { return this.offerListingId; }
            set { this.offerListingId = value; }
        }

        public string Asin
        {
            get { return this.asin; }
            set { this.asin = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public float Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

		public float LowestPrice
		{
            get { return this.lowestPrice; }
            set { this.lowestPrice = value; }
		}

        public float DisplayedPrice
        {
            get { return this.displayedPrice; }
            set { this.displayedPrice = value; }
        }

        public string CurrencySign
        {
            get { return this.currencySign; }
            set { this.currencySign = value; }
        }

        public String UrlItemLink {
            get { return this.urlItemLink; }
            set{ this.urlItemLink = value; }
        }
        public string UrlReview
        {
            get { return this.urlReview; }
            set { this.urlReview = value; }
        }

        public string UrlSmallImage
        {
            get { return this.urlSmallImage; }
            set { this.urlSmallImage = value; }
        }

        public String UrlMediumImage
        {
            get { return this.urlMediumImage; }
            set { this.urlMediumImage = value; }
        }

        public String UrlLargeImage
        {
            get { return this.urlLargeImage; }
            set { this.urlLargeImage = value; }
        }

        public String StarLabel
        {
            get 
            {
                return this.starLabel; 
            }
            set { this.starLabel = value; }
        }

        public float AverageStars{
            get { return this.averageStars; }
            set { this.averageStars = value; }
        }

        public Boolean IsPrimeEligible
        {
            get { return this.isPrimeEligible; }
            set { this.isPrimeEligible = value; }
        }
        public List<ItemImage> ItemImages
        {
            get { return this.itemImages; }
            set { this.itemImages = value; }
        }

        public List<String> Features{
            get { return this.features; }
            set { this.features = value; }
        }

    }
}
