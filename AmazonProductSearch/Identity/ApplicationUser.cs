using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
  
namespace AmazonProductSearch.Identity
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}