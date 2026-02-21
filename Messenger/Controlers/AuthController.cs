using Messenger.DTO;
using Messenger.Services;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<AuthResponce>> Register(RegisterRequest req, CancellationToken ct)
        {
            try
            {
                return Ok(await _service.RegisterAsync(req, ct));
            }
            catch (AuthException ex)
            {
                return Conflict(new ResponseError(ex.Message));//409 
            }
            catch (ValidateException ex)
            {
                return BadRequest(new ResponseError(ex.Message));//400
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseError(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponce>> Login(LoginRequest req, CancellationToken ct)
        {
            try
            {
                return Ok(await _service.LoginAsync(req, ct));

            }
            catch (AuthException ex)
            {
                return Unauthorized(new ResponseError(ex.Message));//401
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseError(ex.Message));//500
            }
        }
    }
}
