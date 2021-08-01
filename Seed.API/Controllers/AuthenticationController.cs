using Microsoft.AspNetCore.Mvc;
using Seed.Interfaces.V1;
using Seed.Models.V1.DTOs;
using System.Threading.Tasks;

namespace Seed.API.Controllers
{
    /// <summary>
    /// This authentication controller should visible for anyone
    /// it does not need to be authorized to be accessed.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _iAuthentication;

        public AuthenticationController(IAuthentication iAuthentication)
        {
            _iAuthentication = iAuthentication;
        }

        /// <summary>
        /// Create a new account
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(UserRequestDto userRequest)
        {
            // Check if the email exist
            var userExists = await _iAuthentication.EmailExist(userRequest.EmailAddress);
            if (userExists.IsError)
                return Ok(userExists);

            // Create a new user with email address
            var userResponse = await _iAuthentication.CreateAccount(userRequest, userRequest.Password);
            if (userResponse.IsError)
            {
                return BadRequest(userResponse.Message);
            }
            else
            {
                return Ok(userResponse.Message);
            }
        }

        /// <summary>
        /// Sign the user in
        /// </summary>
        /// <param name="UserToSignInDto"></param>
        /// <returns></returns>
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(UserToSignInDto userToSignIn)
        {
            var userFromRepo = await _iAuthentication.AccessAccount(userToSignIn.EmailAddress, userToSignIn.Password);
            if (userFromRepo.IsError)
                return BadRequest(userFromRepo);

            return Ok(userFromRepo);
        }
    }
}
