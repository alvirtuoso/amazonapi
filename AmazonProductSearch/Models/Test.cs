using System;
namespace AmazonProductSearch.Models
{
    public class Test
    {
        private string firstName;
        private string lastName;
        public Test()
        {

        }
        public Test(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }
        public String LastName
        {
            get{ return this.lastName; }
            set{ this.lastName = value; }
        }
    }
}
