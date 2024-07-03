using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }

                }
            }

            return BadRequest("Something went wrong!");
        }

        //POST: /api/Auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO) 
        {
            if (loginRequestDTO == null)
            {
                return BadRequest("Request data is null");
            }

            if (userManager == null)
            {
                return StatusCode(500, "User manager is not initialized");
            }

            if (tokenRepository == null)
            {
                return StatusCode(500, "Token repository is not initialized");
            }
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null) 
            {
                // check if the password is right or not 
                var checkPasswordResult=await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPasswordResult) 
                {
                    //Get Roles for this user
                    var roles= await userManager.GetRolesAsync(user);
                    if(roles != null) 
                    {
                        // if everything correct create Token for logged in user
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO 
                        { 
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password incorrect");
        }
    }
}
