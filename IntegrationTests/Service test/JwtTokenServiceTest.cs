using FluentAssertions;
using Messenger.Domain.Entities;
using Messenger.Services;
using Messenger.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Service_test
{
    public class JwtTokenServiceTest
    {
        public static JwtTokenService CreateSut()
        {
            var opt = Options.Create(new JwtOptions
            {
                Issuer = "test-issuer",
                Audience = "test-audince",
                SigningKey = "DtA4wXCgHmy9wi2oHNYJK0t5tyFwtHoUcLVB71S5WpV", 
                AccesTokenMinutes = 60
            });
            return new JwtTokenService(opt);
        }
        [Fact]
        public void CreateAccesToken_returns_valid_token()
        {
            var svc = CreateSut();
            var user = new User
            {
                Username = "alice",
                Email = "any@gmail.com",
                PasswordHash = "qwerty12345"
            };
            var (token, expires) = svc.CreateAccesToken(user);
            token.Should().NotBeNullOrWhiteSpace();
            expires.Should().BeAfter(DateTime.UtcNow);
        }
    }
}
