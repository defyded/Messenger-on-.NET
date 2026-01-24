using Messenger.Domain.Entities;
using Messenger.Services.Interfaces;
using Messenger.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Messenger.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;
        public JwtTokenService(IOptions<JwtOptions> options) => _options = options.Value;
        public (string token, DateTime ExpiresAtUtc) CreateAccesToken(User user)
        {
            var Now = DateTime.UtcNow;
            var expires = Now.AddMinutes(_options.AccesTokenMinutes);
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Username),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(Now).ToUnixTimeSeconds().ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: Now,
                expires: expires,
                signingCredentials: creds
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }
}
