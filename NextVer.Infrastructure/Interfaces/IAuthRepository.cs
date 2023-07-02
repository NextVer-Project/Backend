using NextVer.Domain.Models;

namespace NextVer.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        /*        Task<bool> UpdateUser(User user);*/

        Task<bool> EmailExists(string email);
        Task<bool> ConfirmEmail(string token, string username);
        Task<bool> ConfirmEmailChange(string token, string username, string newEmail);
        /*Task<bool> ResetPassword(string username, string newPassword);
        Task<bool> ConfirmPasswordChange(string token, string username, string newPassword);
        Task<bool> UserHasPrivileges(int userId, int privileges);
        Task<bool> VerifyAuthToken(string token);*/
    }
}
