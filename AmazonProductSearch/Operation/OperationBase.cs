using System;
using System.Collections.Generic;

namespace AmazonProductSearch.Operation
{
    public class OperationBase
    {
        public Dictionary<string, string> StoreInDictionary;
        public OperationBase()
        {
            this.StoreInDictionary = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds the param and value or replace them.
        /// </summary>
        /// <param name="param">Parameter.</param>
        /// <param name="value">Value.</param>
		protected void AddOrReplace(string param, object value)
		{
			if (this.StoreInDictionary.ContainsKey(param))
			{
				this.StoreInDictionary[param] = value.ToString();
			}
			else
			{
				this.StoreInDictionary.Add(param, value.ToString());
			}
		}

        /// <summary>
        /// Add Associates the ID into the dictionary.
        /// </summary>
        /// <param name="associateID">Associate identifier.</param>
		public void AddAssociateTag(string associateID = "alvirtuoso - 20")
		{
			this.AddOrReplace("AssociateTag", associateID);

		}

		/// <summary>
		/// Adds the aws service name.
		/// </summary>
		/// <param name="awsService">Aws service. Default is 'AWSECommerceService'</param>
		public void AddService(string awsService = "AWSECommerceService")
		{
			this.AddOrReplace("Service", awsService);
		}
    }
}
