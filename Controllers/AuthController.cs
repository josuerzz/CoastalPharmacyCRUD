using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Models;
using CoastalPharmacyCRUD.Constants;
using CoastalPharmacyCRUD.Helpers;
using Microsoft.EntityFrameworkCore;
using CoastalPharmacyCRUD.Interfaces;
using System.Text.Json;

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

            if (authResponse == null) return Unauthorized("Some data may be wrong");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = authResponse.RefreshToken.Expires,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("refreshToken", authResponse.RefreshToken.Token, cookieOptions);

            return Ok(new { token = authResponse.Token });
        }

        [HttpPost("logout")]
        public async Task<bool> Logout(Guid userId)
        {
            var user = await _context.SYS_Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            user.RefreshToken = string.Empty;
            user.TokenCreated = DateTime.MinValue;
            user.TokenExpires = DateTime.MinValue;

            _context.SYS_Users.Update(user);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
