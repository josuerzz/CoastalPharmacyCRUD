using CoastalPharmacyCRUD.Constants;
using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Helpers;
using CoastalPharmacyCRUD.Interfaces;
using CoastalPharmacyCRUD.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CoastalPharmacyCRUD.Services
{
    // Service for user registration and login
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ITransactionService _transactionService;
        public AuthService(ApplicationDbContext context, IConfiguration config, ITransactionService transactionService)
        {
            _context = context;
            _config = config;
            _transactionService = transactionService;
        }

        public async Task<SYS_User> Register(UserCreateDto userCreate)
        {

            var userExists = await _context.SYS_Users
                .AnyAsync(u => u.Email.ToLower() == userCreate.Email.ToLower());

            if (userExists) 
            {
                throw new Exception("The email is registered already.");
            }

            // Generating the Refresh Token (7 days)
            var refreshToken = GetRefreshToken();

            var newUser = new SYS_User
            {
                Id = Guid.NewGuid(),
                Email = userCreate.Email,
                PasswordHash = SecurityHelpers.HashPassword(userCreate.Password),
                Name = userCreate.Name,
                Surname = userCreate.Surname,
                Status = 1,
                RoleIdentifierId = StaticData.RoleIdentifiers.User,
                RefreshToken = refreshToken.Token,
                TokenCreated = refreshToken.Created,
                TokenExpires = refreshToken.Expires

            };

            var auditDetails = JsonSerializer.Serialize(new
            {
                newUser.Id,
                newUser.Email,
                newUser.Name,
                newUser.Surname,
                newUser.RoleIdentifierId
            });

            Guid ProcessId = StaticData.TransactionProcesses.CreateUser;

            _context.SYS_Users.Add(newUser);
            _transactionService.SaveTransaction(ProcessId, newUser.Id, "SYS_User", newUser.Id, "A new user was created", auditDetails);

            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<AuthResponseDto?> Login(string email, string password)
        {
            var user = await _context.SYS_Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) { return null ; }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid) { return null; }

            if (user.Status != 1) { throw new Exception("User deactivated, please contact Support"); }

            // Generating the JWT (2h)
            var token = CreateToken(user);

            // Generating the Refresh Token (7 days)
            var refreshToken = GetRefreshToken();

            await SetRefreshToken(refreshToken, user);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken
            };

        }
        /// Generates a JSON Web Token (JWT) for user authentication.
        // The HMAC-SHA512 algorithm is used to ensure integrity.
        private string CreateToken(SYS_User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Surname", user.Surname)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GetRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow

            };

            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken newRefreshToken, SYS_User user)
        {
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            _context.SYS_Users.Update(user);
            await _context.SaveChangesAsync();
        }
    } 
}
