using Hangfire;
using Microsoft.EntityFrameworkCore;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Persistance;
using System.Security.Cryptography;
using System.Text;

namespace NextVer.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly NextVerDbContext _context;
        private const int ExpirationTimeInHours = 3;

        public AuthRepository(NextVerDbContext context)
        {
            _context = context;
        }

        public async Task<User> Register(User user, string password)
        {
            if (user == null || password == null || password == string.Empty)
                return null;

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UserTypeId = 1;
            user.UserLogoUrl = "https://example.com/user1.jpg";
            GenerateConfirmationToken(ref user);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            User user;

            try
            {
                user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            if (user == null)
                return null;

            if (!user.IsVerified)
            {
                GenerateConfirmationToken(ref user);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Username == username);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Email == email);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }
        public async Task<bool> ConfirmEmail(string token, string username)
        {
            User user;
            try
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            if (user == null)
                return false;

            var interval = DateTime.UtcNow - user.ConfirmationTokenGeneratedTime;

            if (interval.Hours >= ExpirationTimeInHours)
                return false;

            if (user.ConfirmationToken != token)
                return false;

            user.ConfirmationToken = null;
            user.IsVerified = true;

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
        public async Task<bool> ChangeEmail(EmailForChange emailForChange)
        {
            var user = await GetById(emailForChange.Id);

            if (user == null)
                return false;

            GenerateConfirmationToken(ref user);

            try
            {
                user.IsVerified = false;
                var expirationTime = user.ConfirmationTokenGeneratedTime.AddHours(ExpirationTimeInHours);
                var waitTime = expirationTime - DateTime.UtcNow;
                BackgroundJob.Schedule(() => CheckEmailConfirmationStatus(user.Id), waitTime);

                return await Update(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> ConfirmEmailChange(string token, string username, string newEmail)
        {
            User user;
            try
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            if (user == null)
                return false;

            var interval = DateTime.UtcNow - user.ConfirmationTokenGeneratedTime;

            if (interval.Hours >= ExpirationTimeInHours)
                return false;

            if (user.ConfirmationToken != token)
                return false;

            user.ConfirmationToken = null;
            user.IsVerified = true;
            user.Email = newEmail;

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        /*public async Task<bool> UpdateUser(User user) { }
        public async Task<bool> DeleteUser(int userId) { }
        public async Task<bool> ResetPassword(string username, string newPassword) { }
        public async Task<bool> ConfirmPasswordChange(string token, string username, string newPassword) { }
        public async Task<bool> UserHasPrivileges(int userId, int privileges) { }
        public async Task<bool> VerifyAuthToken(string token) { }*/
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
        }

        private static void GenerateConfirmationToken(ref User user)
        {
            user.ConfirmationToken = TokenGenerator.GenerateToken();
            user.ConfirmationTokenGeneratedTime = DateTime.UtcNow;
        }
        public async Task<User> GetById(int userId)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<bool> Update(User user)
        {
            try
            {
                _context.Update(user);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public async Task CheckEmailConfirmationStatus(int userId)
        {
            var user = await GetById(userId);

            if (user == null)
                return;

            if (user.IsVerified)
            {
                return;
            }
            else
            {
                user.IsVerified = true;
                await Update(user);
            }
        }
    }
}