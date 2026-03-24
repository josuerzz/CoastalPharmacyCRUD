using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Models;
using CoastalPharmacyCRUD.Constants;
using CoastalPharmacyCRUD.Helpers;
using Microsoft.EntityFrameworkCore;
using CoastalPharmacyCRUD.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;

namespace CoastalPharmacyCRUD.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, ITransactionService transactionService, IAuthService authService)
        {
            _context = context;
            _transactionService = transactionService;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var surname = User.FindFirst("Surname")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var fullname = string.Concat(name, " ", surname);

            return Ok(new 
            { 
                isAuthenticated = true,
                fullName = fullname,
                roleUser = role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto UserDto)
        {
            var user = await _authService.Register(UserDto);

            return Ok(new { message = "User created.", email = user.Email});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var authResponse = await _authService.Login(request.Email, request.Password);

            if (authResponse == null || !authResponse.Success) 
                return Unauthorized(new {message = "Invalid credentials"});

            // Jwt to authorize request (2h)
            var jwtOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(2),
                SameSite = SameSiteMode.Strict
            };
            // Cookie to stay logged in (7days)
            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = authResponse.RefreshToken.Expires,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("X-Access-Token", authResponse.Token, jwtOptions);
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken.Token, refreshOptions);

            return Ok(new 
            {   
                message = "Login Succesful",
                success = true 
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("X-Access-Token");
            Response.Cookies.Delete("X-Refresh-Token");
            return Ok(new { message = "Log out successfully." });
        }
    }
}
