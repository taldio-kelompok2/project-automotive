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

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var response = new ApiResponse<AuthResponseDto>();

            var command = new RegisterCommand(registerRequestDto);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = new ApiResponse<AuthResponseDto>();
            var command = new LoginCommand(loginRequestDto);
            var result = await Mediator!.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var response = new ApiResponse<AuthResponseDto>();

            var accessToken = ExtractAccessTokenFromHeader();

            if (string.IsNullOrEmpty(accessToken))
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = ["Access token is required"];
                return BadRequest(response);
            }

            if (string.IsNullOrEmpty(refreshTokenRequestDto.RefreshToken))
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = ["Refresh token is required"];
                return BadRequest(response);
            }


            var command = new RefreshTokenCommand(refreshTokenRequestDto.RefreshToken, accessToken);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
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

            return Ok(response);
        }

        [HttpPost("send-confirm-email")]
        public async Task<ActionResult<ApiResponse<bool>>> SendConfirmEmail([FromBody] SendConfirmEmailRequestDto request)
        {
            var response = new ApiResponse<bool>();
            var command = new SendConfirmEmailCommand(request.Email);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
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

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = new ApiResponse<bool>();

            var command = new ForgotPasswordCommand(request.Email);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var response = new ApiResponse<bool>();

            var command = new ResetPasswordCommand(request.Email, request.Token, request.NewPassword);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        private string? ExtractAccessTokenFromHeader()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authorizationHeader))
                    return null;

                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return authorizationHeader.Substring("Bearer ".Length).Trim();
                }

                return authorizationHeader;
            }
            catch
            {
                return null;
            }
        }
    }
}
