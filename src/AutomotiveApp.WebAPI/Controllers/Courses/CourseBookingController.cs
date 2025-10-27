using System.Net;
using System.Security.Claims;
using AutomotiveApp.Application.Features.CourseBookings.Commands;
using AutomotiveApp.Application.Features.CourseBookings.Queries;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Helper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.Courses
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class CourseBookingController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseBookingQueryDto>>> GetAll(
            [FromQuery] Guid? sessionId,
            [FromServices] IValidator<GetCourseBookings> validator)
        {
            var response = new ApiResponse<IEnumerable<CourseBookingQueryDto>>();
            var query = new GetCourseBookings(sessionId);
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseBookingQueryDto>> GetById([FromRoute] Guid id,
        [FromServices] IValidator<GetCourseBookingById> validator)
        {
            var query = new GetCourseBookingById(id);
            var response = new ApiResponse<CourseBookingQueryDto>();
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<CourseBookingQueryDto>>> GetCurrentUser([FromQuery] Guid? courseId
        , [FromServices] IValidator<GetCourseBookingByUser> validator)
        {
            var userId = User.GetCurrentUserId() ?? throw new UnauthorizedAccessException();
            var query = new GetCourseBookingByUser(userId, courseId);
            var response = new ApiResponse<IEnumerable<CourseBookingQueryDto>>();
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CourseBookingQueryDto>> Create([FromBody] CourseBookingCommandDto request,
            [FromServices] IValidator<AddCourseBookingCommand> validator)
        {
            var query = new AddCourseBookingCommand(request);
            var response = new ApiResponse<CourseBookingQueryDto>();
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CourseBookingQueryDto>> Update(
        [FromRoute] Guid id,
        [FromBody] Guid newCourseId,
        [FromServices] IValidator<EditCourseBookingCourseCommand> validator)
        {
            var command = new EditCourseBookingCourseCommand(id, newCourseId);
            var response = new ApiResponse<CourseBookingQueryDto>();
            await validator.ValidateAndThrowAsync(command);
            var result = await Mediator.Send(command);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            var command = new DeleteCourseBookingCommand(id);
            await Mediator.Send(command);
            response.Data = $"Course {id} is successfully deleted.";

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
