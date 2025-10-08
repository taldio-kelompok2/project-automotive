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
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<UserQueryDto>>>> GetPagedUsers(
                    [FromQuery] int page = 1,
                    [FromQuery] int itemTaken = 10)
        {
            var response = new ApiResponse<PaginatedResult<UserQueryDto>>();

            try
            {
                var query = new GetUsersPaged(page, itemTaken);
                var result = await _mediator.Send(query);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.InternalServerError;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<UserQueryDto>>> GetUserById(Guid userId)
        {
            var response = new ApiResponse<UserQueryDto>();

            try
            {
                var query = new GetUserById(userId);
                var result = await _mediator.Send(query);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.NotFound;
                response.Errors = [ex.Message];
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.InternalServerError;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            var response = new ApiResponse<Guid>();

            try
            {
                var command = new CreateUser(userCreateDto);
                var result = await _mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.Created;
                response.Data = result;

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        [HttpPut("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(
            Guid userId,
            [FromBody] UserUpdateDto userUpdateDto)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new UpdateUser(userId, userUpdateDto);
                var result = await _mediator.Send(command);

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

        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid userId)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var command = new DeleteUser(userId);
                var result = await _mediator.Send(command);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.NotFound;
                response.Errors = [ex.Message];
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.InternalServerError;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }
    }
}
