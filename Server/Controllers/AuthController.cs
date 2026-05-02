using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Auth;
using Server.DTOs.Common;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        public AuthController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser(RegisterRequest request,CancellationToken ct)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _service.RegisterUserAsync(request,ip!,ct);
            if(result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "User can't Register please check the credantials.",
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = 201,
                Success = true,
                Message = "User Register Successfully.",
                Data = result
            });
        }
        [HttpPost("/login")]
        public async Task<IActionResult> LoginUser(LoginRequest request,CancellationToken ct)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _service.LoginUserAsync(request,ip!,ct);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid credentials"
                });

            }
            return Ok(new ApiResponse
            {
                Code = 200,
                Success = true,
                Message = "Login successful",
                Data = new
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken
                }
            });
        }
    }
}
