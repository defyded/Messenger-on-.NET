using Messenger.Domain.Entities;
using Messenger.DTO;
using Messenger.Infastructure.Persistence;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Services
{
    public class AuthService : IAuthService
    {
        private readonly MessengerDBContext _context;
        private readonly ITokenService _tokens;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(MessengerDBContext context, ITokenService tokens)
        {
            _context = context;
            _tokens = tokens;
        }

        public async Task<AuthResponce> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var username = Normalize(request.Username);
            //здест тоже надо добавить
            var email = request.Email;
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email, ct);
            if(user is null)
                { throw new AuthException("INVALID_CREDENTIALS", "Неверный логин или пароль."); }

            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            
            if(res == PasswordVerificationResult.Failed)
                { throw new AuthException("INVALID_CREDENTIALS", "Неверный логин или пароль."); }

            //нет поля для даты последнего логина 
            await _context.SaveChangesAsync();

            var (token, exp) = _tokens.CreateAccesToken(user);

            return new AuthResponce(user.Id, user.Username, user.Email, token, exp);
        }

        public async Task<AuthResponce> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            var username = Normalize(request.Username);
            //добавить нормализацию почты 
            var email = request.Email;
            ValidateCredentials(username, request.Password);

            var exists = await _context.Users.AnyAsync(x => x.Username == username || x.Email == email, ct);
            if (exists)
                { throw new AuthException("USERNAME_OR_EMAIL_TAKEN", "This username or email are taken"); }
            
            var user = new User { 
                Username = username,
                Email = email
            };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            var (token, exp) = _tokens.CreateAccesToken(user);
            return new AuthResponce(user.Id, user.Username, user.Email, token, exp);

        }

        private static string Normalize(string username) => (username ?? "").Trim().ToLowerInvariant();
        private static void ValidateCredentials(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
                { throw new AuthException("INVALID_USERNAME", "Username must be 3 - 50 symbols"); }

            if (String.IsNullOrWhiteSpace(password) || password.Length < 8)
                { throw new AuthException("WEAK_PASSWORD", "Password must be more than 8 symbols"); }
        }

    }
    public sealed class AuthException : Exception
    {
        public string Code { get; }

        public AuthException(string code, string message) : base(message)
            => Code = code;
    }
}
