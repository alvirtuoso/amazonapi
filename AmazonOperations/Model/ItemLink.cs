

namespace AmazonOperations.Model
{
    public class ItemLink
    {
        private string _url;

        public string Description { get; set; }
        public string URL
        {
            get { return this._url; }
            set { this._url = System.Net.WebUtility.UrlDecode(value); }
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
