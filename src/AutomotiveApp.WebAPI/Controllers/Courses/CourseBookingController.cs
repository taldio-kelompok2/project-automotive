using AutomotiveApp.Application.Features.CourseBookings.Commands;
using AutomotiveApp.Application.Features.CourseBookings.Queries;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.Courses
{
    [ApiController]
    [Route("api/[controller]")]

    public class CourseBookingController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseBookingQueryDto>>> GetCourseBookings(
            [FromQuery] Guid? sessionId,
            [FromServices] IValidator<GetCourseBookings> validator)
        {
            var response = new ApiResponse<IEnumerable<CourseBookingQueryDto>>();
            var query = new GetCourseBookings(sessionId);
            var validation = await validator.ValidateAsync(query);

            if (!validation.IsValid) return BadRequest(new ApiResponse<IEnumerable<CourseBookingQueryDto>>
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
        public async Task<ActionResult<CourseBookingQueryDto>> GetCourseBookingById([FromRoute] Guid id, [FromServices] IValidator<GetCourseBookingById> validator)
        {
            var query = new GetCourseBookingById(id);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseBookingQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseBookingQueryDto>(validation);

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
        public async Task<ActionResult<CourseBookingQueryDto>> CreateCourseBooking([FromBody] CourseBookingCommandDto request,
            [FromServices] IValidator<AddCourseBookingCommand> validator)
        {
            var query = new AddCourseBookingCommand(request);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseBookingQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseBookingQueryDto>(validation);

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

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CourseBookingQueryDto>> UpdateCourseBookingSession(
        [FromRoute] Guid id,
        [FromBody] Guid newCourseId,
        [FromServices] IValidator<EditCourseBookingCourseCommand> validator)
        {
            var command = new EditCourseBookingCourseCommand(id, newCourseId);
            var response = new ApiResponse<CourseBookingQueryDto>();

            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
                return HandleValidationFailure<CourseBookingQueryDto>(validation);

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
                response.Errors = new[] { ex.Message };

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourseBooking([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            try
            {
                var command = new DeleteCourseBookingCommand(id);
                await Mediator.Send(command);
                response.Data = $"Course {id} is successfully deleted.";

                response.Success = true;
                response.StatusCode = HttpCode.OK;

                return Ok(response);
            }
            catch (NotFoundException<CourseBooking> ex)
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