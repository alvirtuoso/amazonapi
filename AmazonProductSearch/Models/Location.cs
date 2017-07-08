using System;
namespace AmazonProductSearch.Models
{
    public class Location
    {
        //private string ip;
        //private string countryCode;
        //private string countryName;
        //private string regionCode;
        //private string regionName;
        //private string city;
        //private string zipCode;
        //private string timeZone;
        //private string latitude;
        //private string longitude;
        //private string metroCode;

        public Location()
        {
        }

        public String IP
        {
            get;
            set;
        }

        public String Country_Code
        {
            get;
            set;
        }

        public String Country_Name
        {
            get;
            set;
        }

        public String Region_Code
        {
            get;
            set;
        }
        public String Region_Name
        {
            get;
            set;
        }
        public String City
        {
            get;
            set;
        }
        public String Zip_Code{
            get;
            set;
        }
        public String Time_Zone
        {
            get;
            set;
        }
        public String Latitude
        {
            get;
            set;
        }
        public String Longitude{
            get; set;
        }
        public String Metro_Code
        {
            get;
            set;
        }
    }
}
