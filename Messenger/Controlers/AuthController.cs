using Messenger.DTO;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controlers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service) => _service = service;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponce>> Register(RegisterRequest req, CancellationToken ct) => Ok(await _service.RegisterAsync(req, ct));

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponce>> Login(LoginRequest req, CancellationToken ct) => Ok(await _service.LoginAsync(req, ct));
    }
}
