using System;
namespace AmazonProductSearch.Models
{
    /// <summary>
    /// Item image.
    /// </summary>
    public class ItemImage
    {
        private string urlThumbnail;
        private string urlLargeImage;

        public ItemImage()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AmazonProductSearch.Models.ItemImage"/> class.
        /// </summary>
        /// <param name="urlThumbnail">URL thumbnail.</param>
        /// <param name="urlLargeImage">URL large image.</param>
        public ItemImage(string urlThumbnail, string urlLargeImage)
        {
            this.urlThumbnail = urlThumbnail;
            this.urlLargeImage = urlLargeImage;
        }

        /// <summary>
        /// Gets or sets the URL thumbnail.
        /// </summary>
        /// <value>The URL thumbnail.</value>
        public String UrlThumbnail
        {
            get { return this.urlThumbnail; }
            set { this.urlThumbnail = value; }
        }

        /// <summary>
        /// Gets or sets the URL large image.
        /// </summary>
        /// <value>The URL large image.</value>
		public String UrlLargeImage
		{
            get { return this.urlLargeImage; }
            set { this.urlLargeImage = value; }
		}
    }
}
