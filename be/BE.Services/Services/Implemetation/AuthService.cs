using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.GenericRepository;
using BE.DAL.Models;
using BE.DAL.UOW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BE.Services.Services.Implemetation
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(
            IServiceProvider serviceProvider,
            IGenericRepository<User> userRepo,
            IGenericRepository<RefreshToken> refreshTokenRepo,
            IUnitOfWork unitOfWork,
            IConfiguration configuration) : base(serviceProvider)
        {
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByExpression(u => u.Email == request.Email,null);
            
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is inactive");
            }

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);

            await _refreshTokenRepo.Insert(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // JWT expires in 1 hour
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                }
            };
        }

        public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepo.GetByExpression(u => u.Email == request.Email,null);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Name = request.Name,
                Role = request.Role,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepo.Insert(user);
            await _unitOfWork.SaveChangesAsync();

            // Auto-login after registration
            return await LoginAsync(new LoginRequest { Email = request.Email, Password = request.Password });
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepo.GetByExpression(rt => rt.Token == refreshToken,null);
            
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = await _userRepo.GetById(storedToken.UserId);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            // Revoke old refresh token
            storedToken.IsRevoked = true;
            await _refreshTokenRepo.Update(storedToken);

            // Generate new tokens
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);

            await _refreshTokenRepo.Insert(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                }
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepo.GetByExpression(rt => rt.Token == refreshToken, null);
            if (storedToken == null)
                return false;

            storedToken.IsRevoked = true;
            await _refreshTokenRepo.Update(storedToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserInfo> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepo.GetById(userId);
            if (user == null)
                return null;

            return new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(Guid userId)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Refresh token expires in 7 days
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                IsRevoked = false
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
