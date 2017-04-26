using System;

namespace AmazonProductSearch.Models
{
    /// <summary>
    /// Item.
    /// </summary>
    public class Item
    {
        private string title;
        private string price;
        private string urlReview;
        private string urlSmallImage;
        private string urlMediumImage;
        private string urlLargeImage;
        private string starLabel;
        private float averageStars;

        public Item(){}
        public Item(string title, string price, string urlReview, string urlSmallImage, string urlMediumImage, string urlLargeImage, string stars)
        {
            this.title = title;
            this.price = price;
            this.urlReview = urlReview;
            this.urlSmallImage = urlSmallImage;
            this.urlMediumImage = urlMediumImage;
            this.urlLargeImage = urlLargeImage;
            this.starLabel = stars;
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Price
        {
            get { return this.price; }
            set { this.price = value; }
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
    }
}
