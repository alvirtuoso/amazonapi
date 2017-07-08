using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth.Unofficial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmazonProductSearch.Controllers
{
    [Route("account/[controller]")]
    public class AccountController : Controller
    {
		private readonly IFirebaseAuthProvider _firebaseAuthProvider;

		public AccountController(IFirebaseAuthProvider firebaseAuthProvider)
		{
			_firebaseAuthProvider = firebaseAuthProvider;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]Models.LoginRequestBody request)
		{
			try
			{
				var result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(request.Email, request.Password);

				return Ok(result);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
