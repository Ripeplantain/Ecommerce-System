using System.Security.Claims;
using Ecommerce.User.Database;
using Ecommerce.User.Dto;
using Ecommerce.User.Entities;
using Ecommerce.User.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.User.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController(
        ILogger<AccountController> logger,
        UserManager<AppUser> userManager,
        ITokenRepository tokenRepository
        ) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenRepository _tokenRepository = tokenRepository;

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto input)
        {
            try {
                var user = new AppUser {
                    FullName = input.FullName,
                    Email = input.Email,
                    UserName = input.Email,
                    PhoneNumber = input.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, input.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return StatusCode(201, user);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error while registering user");
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto input)
        {
            try {
                var user = await _userManager.FindByEmailAsync(input.Email);

                if (user == null)
                {
                    return BadRequest("Invalid credentials");
                }

                var result = await _userManager.CheckPasswordAsync(user, input.Password);

                if (!result)
                {
                    return BadRequest("Invalid credentials");
                }

                _logger.LogInformation(user.Id.ToString());

                var accessToken = _tokenRepository.GenerateAccessToken(user);
                var refreshToken = await _tokenRepository.GenerateRefreshToken(user);

                return StatusCode(201, new { 
                    accessToken,
                    refreshToken,
                    user
                });
            } catch (Exception ex) {
                _logger.LogError(ex, "Error while logging in user");
                return BadRequest();
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(string input)
        {
            try {
                await _tokenRepository.ValidateRefreshToken(input);

                var claimsPrincipal = await _tokenRepository.GetClaimsPrincipalFromToken(input);
                var user = await _userManager.FindByIdAsync(claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value) ?? throw new SecurityTokenException("Invalid token");
                var accessToken = _tokenRepository!.GenerateAccessToken(user);
                var refreshToken = await _tokenRepository.GenerateRefreshToken(user);

                return StatusCode(201, new { 
                    accessToken,
                    refreshToken,
                    user
                });
            } catch (Exception ex) {
                _logger.LogError(ex, "Error while refreshing token");
                return BadRequest();
            }
        }
    }

}