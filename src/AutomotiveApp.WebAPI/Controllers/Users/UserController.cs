using System.Net;
using AutomotiveApp.Application.Features.Users.Commands;
using AutomotiveApp.Application.Features.Users.Queries;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator _mediator) : BaseApiController(_mediator)
    {
        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetCurrentUser()
        {
            var response = new ApiResponse<UserProfileDto>();

            try
            {
                var accessToken = ExtractAccessTokenFromHeader();
                Console.WriteLine($"[getcurruser] token: {accessToken}");
                if (string.IsNullOrEmpty(accessToken))
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = ["Access token is required"];
                    return BadRequest(response);
                }

                var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(stringUserId))
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = ["Invalid JWT"];
                    return BadRequest(response);
                }
                var userId = Guid.Parse(stringUserId);

                var query = new GetUserById(userId);
                var result = await _mediator.Send(query);

                response.Success = true;
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.Errors = [ex.Message];
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> CreateUser([FromBody] UserCreateRequestDto userCreateDto)
        {
            var response = new ApiResponse<bool>();

            var command = new CreateUser(userCreateDto);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.StatusCode = HttpStatusCode.Created;
            response.Data = result;

            return StatusCode(201, response);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<UserQueryDto>>>> GetPagedUsers(
                    [FromQuery] int page = 1,
                    [FromQuery] int itemTaken = 10,
                    [FromQuery] string? search = null)
        {
            var response = new ApiResponse<PaginatedResult<UserQueryDto>>();

            var query = new GetUsersPaged(page, itemTaken, search);
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserQueryDto>>>> GetAllUsers()
        {
            var response = new ApiResponse<IEnumerable<UserQueryDto>>();

            var query = new GetAllUsers();
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetUserById(Guid userId)
        {
            var response = new ApiResponse<UserProfileDto>();

            var query = new GetUserById(userId);
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }


        [HttpPut("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(
            Guid userId,
            [FromBody] UserUpdateRequestDto userUpdateDto)
        {
            var response = new ApiResponse<bool>();

            var command = new UpdateUser(userId, userUpdateDto);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid userId)
        {
            var response = new ApiResponse<bool>();

            var command = new DeleteUser(userId);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }
    }
}
