using AutomotiveApp.Application.Features.Auth.Command;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : BaseApiController(mediator)
    {

        [HttpPost("send-confirm-email")]
        public async Task<ActionResult<ApiResponse<bool>>> SendConfirmEmail([FromBody] SendConfirmEmailDto request)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new SendConfirmEmailCommand(request.Email);
                var result = await Mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        // TODO: change to POST & use DTO when frontend is implemented
        [HttpGet("confirm-email")]
        public async Task<ActionResult<ApiResponse<bool>>> ConfirmEmail(
            [FromQuery] string userId,
            [FromQuery] string token)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new ConfirmEmailCommand(userId, token);
                var result = await Mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new ForgotPasswordCommand(request.Email);
                var result = await Mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        // TODO: test when frontend is implemented
        // currently the link is only a GET request
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new ResetPasswordCommand(request.Email, request.Token, request.NewPassword);
                var result = await Mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }
    }
}
