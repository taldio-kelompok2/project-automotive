using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Features.CourseSessions.Commands;
using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Dto.Courses;
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
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<IEnumerable<CourseSessionQueryDto>>();

            if (!validation.IsValid) return BadRequest(new ApiResponse<IEnumerable<CourseSessionQueryDto>>
            {
                Success = false,
                StatusCode = HttpCode.BadRequest,
                Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
            });

            try
            {
                var result = await Mediator.Send(query);

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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseSessionQueryDto>> GetSessionById([FromRoute] Guid id,
        [FromServices] IValidator<GetCourseSessionById> validator)
        {
            var query = new GetCourseSessionById(id);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseSessionQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseSessionQueryDto>(validation);

            try
            {
                var result = await Mediator.Send(query);

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

        [HttpPost]
        public async Task<ActionResult<CourseSessionQueryDto>> CreateSession([FromBody] CourseSessionCommandDto request,
        [FromServices] IValidator<AddCourseSessionCommand> validator)
        {
            var query = new AddCourseSessionCommand(request);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseSessionQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseSessionQueryDto>(validation);

            try
            {
                var result = await Mediator.Send(query);

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

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CourseSessionQueryDto>> EditSession([FromBody] CourseSessionEditCommandDto request,
        [FromRoute] Guid id,
        [FromServices] IValidator<EditCourseSessionCommand> validator)
        {
            var command = new EditCourseSessionCommand(request);
            var response = new ApiResponse<CourseSessionQueryDto>();
            request.Id = id;
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid) return HandleValidationFailure<CourseSessionQueryDto>(validation);

            try
            {
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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteSession([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            try
            {
                var command = new DeleteCourseSessionCommand(id);
                await Mediator.Send(command);
                response.Data = $"Course {id} is successfully deleted.";

                response.Success = true;
                response.StatusCode = HttpCode.OK;

                return Ok(response);
            }
            catch (NotFoundException<Course> ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.NotFound;
                response.Errors = [ex.Message];
                return NotFound(response);
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