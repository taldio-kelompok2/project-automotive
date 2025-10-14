
using System.Net;
using AutomotiveApp.Application.Features.CourseSessions.Commands;
using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.Courses
{
    [ApiController]
    [Route("api/[controller]")]

    public class CourseSessionController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseSessionQueryDto>>> GetSessions([FromQuery] Guid? courseId,
        [FromServices] IValidator<GetCourseSessions> validator)
        {
            var query = new GetCourseSessions(courseId);
            await validator.ValidateAndThrowAsync(query);
            var response = new ApiResponse<IEnumerable<CourseSessionQueryDto>>();

            var result = await Mediator.Send(query);
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseSessionQueryDto>> GetSessionById([FromRoute] Guid id,
        [FromServices] IValidator<GetCourseSessionById> validator)
        {
            var query = new GetCourseSessionById(id);
            await validator.ValidateAndThrowAsync(query);
            var response = new ApiResponse<CourseSessionQueryDto>();

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CourseSessionQueryDto>> CreateSession([FromBody] CourseSessionCommandDto request,
        [FromServices] IValidator<AddCourseSessionCommand> validator)
        {
            var query = new AddCourseSessionCommand(request);
            await validator.ValidateAndThrowAsync(query);
            var response = new ApiResponse<CourseSessionQueryDto>();

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CourseSessionQueryDto>> EditSession([FromBody] CourseSessionEditCommandDto request,
        [FromRoute] Guid id,
        [FromServices] IValidator<EditCourseSessionCommand> validator)
        {
            var command = new EditCourseSessionCommand(request);
            var response = new ApiResponse<CourseSessionQueryDto>();
            request.Id = id;
            await validator.ValidateAndThrowAsync(command);

            var result = await Mediator.Send(command);
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteSession([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            var command = new DeleteCourseSessionCommand(id);
            await Mediator.Send(command);
            response.Data = $"Course {id} is successfully deleted.";

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }
}