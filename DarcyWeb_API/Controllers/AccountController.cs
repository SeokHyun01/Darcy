using Darcy_Common;
using Darcy_DataAccess;
using Darcy_Models;
using Darcy_Models.Authentication;
using DarcyWeb_API.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DarcyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<AccountController> _logger;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly APISettings _aPISettings;


		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ILogger<AccountController> logger,
			RoleManager<IdentityRole> roleManager,
			IOptions<APISettings> options)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_roleManager = roleManager;
			_aPISettings = options.Value;
		}

		[HttpPost]
		public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequest)
		{
			if (signUpRequest == null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var user = new ApplicationUser
			{
				UserName = signUpRequest.Email,
				Email = signUpRequest.Email,
				Name = signUpRequest.Name,
				//PhoneNumber = signUpRequest.PhoneNumber,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(user, signUpRequest.Password);

			if (!result.Succeeded)
			{
				return BadRequest(new SignUpResponseDTO()
				{
					IsSuccessful = false,
					Errors = result.Errors.Select(u => u.Description)
				});
			}

			var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Client);
			if (!roleResult.Succeeded)
			{
				return BadRequest(new SignUpResponseDTO()
				{
					IsSuccessful = false,
					Errors = result.Errors.Select(u => u.Description)
				});
			}
			return StatusCode(201);
		}


		[HttpPost]
		public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequest)
		{
			if (signInRequest == null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var result = await _signInManager.PasswordSignInAsync(signInRequest.UserName, signInRequest.Password, false, false);
			if (result.Succeeded)
			{
				var user = await _userManager.FindByNameAsync(signInRequest.UserName);
				if (user == null)
				{
					return Unauthorized(new SignInResponseDTO
					{
						IsAuthSuccessful = false,
						ErrorMessage = "Invalid Authentication"
					});
				}

				// Everything is valid and we need to login
				var signinCredentials = GetSigningCredentials();
				var claims = await GetClaims(user);

				var tokenOptions = new JwtSecurityToken(
					issuer: _aPISettings.ValidIssuer,
					audience: _aPISettings.ValidAudience,
					claims: claims,
					expires: DateTime.Now.AddDays(30),
					signingCredentials: signinCredentials);

				var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

				return Ok(new SignInResponseDTO()
				{
					IsAuthSuccessful = true,
					Token = token,
					UserDTO = new UserDTO()
					{
						Id = user.Id,
						Name = user.Name,
						Email = user.Email,
					}
				});

			}
			else
			{
				return Unauthorized(new SignInResponseDTO
				{
					IsAuthSuccessful = false,
					ErrorMessage = "Invalid Authentication"
				});
			}

			return StatusCode(201);
		}


		private SigningCredentials GetSigningCredentials()
		{
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_aPISettings.SecretKey));

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}


		private async Task<List<Claim>> GetClaims(ApplicationUser user)
		{
			var claims = new List<Claim>
			{
				new Claim("Key", user.Id),
				new Claim("Id", user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, user.Name)
			};

			var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			return claims;
		}
	}
}
