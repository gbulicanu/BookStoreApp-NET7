using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(
        ILogger<AuthController> logger,
        IConfiguration configuration,
        IMapper mapper,
        UserManager<ApiUser> userManager)
    {
        _logger = logger;
        _configuration = configuration;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        _logger.LogInformation("Registration attempt for {User}", userDto.Email);
        var user = _mapper.Map<ApiUser>(userDto);
        user.UserName = userDto.Email;
        var result = await _userManager.CreateAsync(user, userDto.Password);

        try
        {
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(userDto.Role))
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
            }

            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something wen wrong in the {Method}", nameof(Register));
            return Problem($"Something wen wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
    {
        _logger.LogInformation("Login attempt for {User}", userDto.Email);
        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user!, userDto.Password);

            if (user == null || !passwordValid)
            {
                return Unauthorized(userDto);
            }

            string tokenString = await GenerateTokenAsync(user);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Email = userDto.Email,
                Token = tokenString,
            };

            return Accepted(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something wen wrong in the {Method}", nameof(Login));
            return Problem($"Something wen wrong in the {nameof(Login)}", statusCode: 500);
        }
    }

    private async Task<string> GenerateTokenAsync(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
        var userClaims = await _userManager.GetClaimsAsync(user);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(CustomClaimNames.Uid, user.Id)
        }
        .Union(roleClaims)
        .Union(userClaims);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
