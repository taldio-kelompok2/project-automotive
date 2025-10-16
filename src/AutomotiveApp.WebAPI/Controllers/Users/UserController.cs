using System.Net;
using AutomotiveApp.Application.Features.Users.Commands;
using AutomotiveApp.Application.Features.Users.Queries;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator _mediator) : BaseApiController(_mediator)
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateUser([FromBody] UserCreateRequestDto userCreateDto)
        {
            var response = new ApiResponse<Guid>();

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
                    [FromQuery] int itemTaken = 10)
        {
            var response = new ApiResponse<PaginatedResult<UserQueryDto>>();

            var query = new GetUsersPaged(page, itemTaken);
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<UserQueryDto>>> GetUserById(Guid userId)
        {
            var response = new ApiResponse<UserQueryDto>();

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
