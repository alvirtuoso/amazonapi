
namespace AmazonProductSearch.Helpers
{
    internal static class AmazonDomain
    {
		internal static string GetDomain(string countryCode)
		{
			switch (countryCode)
			{
				case "BR":
					return "amazon.com.br";
				case "CA":
					return "amazon.ca";
				case "CN":
					return "amazon.cn";
				case "DE":
					return "amazon.de";
				case "ES":
					return "amazon.es";
				case "FR":
					return "amazon.fr";
				case "IN":
					return "amazon.in";
				case "IT":
					return "amazon.it";
				case "JP":
					return "amazon.co.jp";
				case "MX":
					return "amazon.com.mx";
				case "UK":
					return "amazon.co.uk";
				case "US":
					return "amazon.com";
				default:
					return "amazon.com";
			}
		}

        internal static string GetDomain(AmazonEndpoint amazonEndpoint)
        {
            switch (amazonEndpoint)
            {
                case AmazonEndpoint.BR:
                    return "amazon.com.br";
                case AmazonEndpoint.CA:
                    return "amazon.ca";
                case AmazonEndpoint.CN:
                    return "amazon.cn";
                case AmazonEndpoint.DE:
                    return "amazon.de";
                case AmazonEndpoint.ES:
                    return "amazon.es";
                case AmazonEndpoint.FR:
                    return "amazon.fr";
                case AmazonEndpoint.IN:
                    return "amazon.in";
                case AmazonEndpoint.IT:
                    return "amazon.it";
                case AmazonEndpoint.JP:
                    return "amazon.co.jp";
                case AmazonEndpoint.MX:
                    return "amazon.com.mx";
                case AmazonEndpoint.UK:
                    return "amazon.co.uk";
                case AmazonEndpoint.US:
                    return "amazon.com";
                default:
                    return "amazon.com";
            }
        }
    }
}