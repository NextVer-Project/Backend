using NextVer.Domain.Models;

namespace NextVer.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<bool> EmailExists(string email);
        Task<bool> ConfirmEmail(string token, string username);
        //Task<bool> ConfirmEmailChange(string token, string username, string newEmail);
        Task<User> GetById(int userId);
        Task<bool> CheckAndRenewActivationLink(User user);
    }
}