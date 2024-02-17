using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] ResgisterRequestDto request)
        {
            var user = new IdentityUser
            {
                Email = request.Email?.Trim(),
                UserName = request.Email?.Trim()
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if(identityResult.Succeeded)
                {
                    return Ok();
                }
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach(var error in  identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if( identityUser is not null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);                

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    var token = tokenRepository.CreateToken(identityUser,roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = token
                    };

                    return Ok(response);
                }                
            }

            ModelState.AddModelError("", "Email or password incorrect!");

            return ValidationProblem(ModelState);
        }
    }
}
