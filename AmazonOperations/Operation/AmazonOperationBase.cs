﻿using AmazonOperations.Model;
using System.Collections.Generic;

namespace AmazonOperations.Operation
{
    public class AmazonOperationBase
    {
        public Dictionary<string, string> ParameterDictionary;

        public AmazonOperationBase()
        {
            this.ParameterDictionary = new Dictionary<string, string>();
        }

        public void ResponseGroup(AmazonResponseGroup responseGroup)
        {
            this.AddOrReplace("ResponseGroup", responseGroup.ToString());
        }

        public void SearchIndex(AmazonSearchIndex searchIndex)
        {
            this.AddOrReplace("SearchIndex", searchIndex.ToString());
        }

        public void AssociateTag(string associateTag)
        {
            this.AddOrReplace("AssociateTag", associateTag);
        }

        protected void AddOrReplace(string param, object value)
        {
            if (this.ParameterDictionary.ContainsKey(param))
            {
                this.ParameterDictionary[param] = value.ToString();
            }
            else
            {
                this.ParameterDictionary.Add(param, value.ToString());
            }
        }
    }
}
