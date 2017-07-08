using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AmazonProductSearch.Identity
{
	public class ApplicationRole : IdentityRole
	{
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public string IPAddress { get; set; }
	}
}
