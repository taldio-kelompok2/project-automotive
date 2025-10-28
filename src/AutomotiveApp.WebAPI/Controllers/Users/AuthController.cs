using AutomotiveApp.Application.Features.Auth.Command;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : BaseApiController(mediator)
    {

        private static void SetCookies(HttpResponse response, AuthResponseDto dto)
        {
            // Access Token cookie
            response.Cookies.Append("AuthToken", dto.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = dto.AccessTokenExpiry,
                Path = "/"
            });

            // Refresh Token cookie
            response.Cookies.Append("RefreshToken", dto.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            });
        }

        private static void ClearCookies(HttpResponse response)
        {
            response.Cookies.Delete("AuthToken", new CookieOptions { Path = "/" });
            response.Cookies.Delete("RefreshToken", new CookieOptions { Path = "/" });
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var response = new ApiResponse<AuthResponseDto>();

            var command = new RegisterCommand(registerRequestDto);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.StatusCode = HttpStatusCode.Created;
            response.Data = result;

            if (result.Success)
            {
                // SetCookies(Response, result);
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = new ApiResponse<AuthResponseDto>();

            var command = new LoginCommand(loginRequestDto);
            var result = await Mediator.Send(command);

            response.Success = result.Success;
            response.Data = result;

            if (result.Success)
            {
                SetCookies(Response, result);
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken()
        {
            var response = new ApiResponse<AuthResponseDto>();

            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                response.Success = false;
                response.Data = new AuthResponseDto();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = ["Refresh token is required"];
                return BadRequest(response);
            }

            var command = new RefreshTokenCommand(refreshToken);
            var result = await Mediator.Send(command);

            if (result.Success)
            {
                SetCookies(Response, result);
            }

            response.Success = result.Success;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            if (result.Success)
            {
                SetCookies(Response, result);
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Logout()
        {
            var response = new ApiResponse<string>();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.Success = true;
                return Ok(response);
            }

            var command = new LogoutCommand(userId);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = "Logout successful";

            if (result)
            {
                ClearCookies(Response);
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult AuthCheck()
        {
            return Ok(new { status = "authenticated" });
        }

        [HttpPost("send-confirm-email")]
        public async Task<ActionResult<ApiResponse<bool>>> SendConfirmEmail([FromBody] SendConfirmEmailRequestDto request)
        {
            var response = new ApiResponse<bool>();
            var command = new SendConfirmEmailCommand(request.Email);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            if (result) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<ApiResponse<bool>>> ConfirmEmail(
            [FromQuery] string userId,
            [FromQuery] string token)
        {
            var response = new ApiResponse<bool>();
            var command = new ConfirmEmailCommand(userId, token);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            if (result) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var command = new ForgotPasswordCommand(request.Email);
                var result = await Mediator.Send(command);

                response.Success = true;
                response.Data = result;

                if (result) return Ok(response);
                return BadRequest(response);
            }
            catch (Exception ex) // temp: biar gak expose email yg ada
            {
                Console.WriteLine($"forgot password error: {ex.Message}");

                response.Success = true;
                return BadRequest(response);
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var response = new ApiResponse<bool>();

            var command = new ResetPasswordCommand(request.Email, request.Token, request.NewPassword);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            if (result) return Ok(response);
            return BadRequest(response);
        }
    }
}
