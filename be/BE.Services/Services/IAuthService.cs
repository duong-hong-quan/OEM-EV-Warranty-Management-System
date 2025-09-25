using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Common;

namespace BE.Services.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserInfo> GetUserByIdAsync(Guid userId);
    }
}
