using Internet_banking.Core.Application.Dtos.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> AuthenticationAsync(AuthenticationRequest request);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPassWordResponse> ForgotPasswordRequestAsync(ForgotPassowordRequest request, string origin);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<List<AuthenticationResponse>> GetAllUsersAsync();
        Task<AuthenticationResponse> GetUserByIdAsync(string id);
        Task<AuthenticationResponse> IsVerified(string id);
        Task<RegisterResponse> UpdateUserAsync(RegisterRequest request);

        Task SignOutAsync();
    }
}