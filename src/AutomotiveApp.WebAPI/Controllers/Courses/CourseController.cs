using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Features.Courses.Queries;
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

    public class CourseController(IMediator mediator, IMapper mapper, IFileStorage ImageStorage) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseQueryDto>>> GetCourses()
        {
            var response = new ApiResponse<IEnumerable<CourseQueryDto>>();

            try
            {
                var query = new GetCourses();
                var result = await _mediator.Send(query);

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
        public async Task<ActionResult<CourseQueryDetailDto>> GetCourseById([FromRoute] Guid id,
        [FromServices] IValidator<GetCourseById> validator)
        {
            var query = new GetCourseById(id);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseQueryDetailDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseQueryDetailDto>(validation);

            try
            {
                var result = await _mediator.Send(query);

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

        [HttpGet("paged")]
        public async Task<ActionResult<PaginatedResult<CourseQueryDto>>> GetPagedCourses(
            [FromQuery] int page = 1,
            [FromQuery] int itemTaken = 6
            )
        {
            var response = new ApiResponse<PaginatedResult<CourseQueryDto>>();

            try
            {
                var query = new GetCoursesPaged(page, itemTaken);
                var result = await _mediator.Send(query);

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
        public async Task<ActionResult<CourseQueryDto>> AddCourse([FromForm] CourseCreateRequest request,
        [FromServices] IValidator<CourseCreateRequest> validator)
        {
            var response = new ApiResponse<CourseQueryDto>();
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CourseQueryDto>(validation);
            string? imageFileName = null;
            var dto = mapper.Map<CourseCommandDto>(request);
            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{request.Id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.SaveFileAsync<Course>(request.Image.OpenReadStream(), imageFileName);
                }

                dto.ImageFilename = imageFileName;
                var command = new AddCourseCommand(dto);
                var result = await _mediator.Send(command);
                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (imageFileName != null)
                {
                    await ImageStorage.DeleteFileAsync<Course>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CourseQueryDto>> EditCourse([FromRoute] Guid id, [FromForm] CourseEditRequest request,
        IValidator<CourseEditRequest> validator)
        {
            var response = new ApiResponse<CourseQueryDto>();
            request.Id = id;
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CourseQueryDto>(validation);

            string? imageFileName = null;
            var dto = mapper.Map<CourseCommandEditDto>(request);
            var command = new EditCourseCommand(dto);

            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.ReplaceFileAsync<Course>(imageFileName, request.Image.OpenReadStream());
                }
                dto.ImageFilename = imageFileName;
                var result = await _mediator.Send(command);
                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (imageFileName != null)
                {
                    await ImageStorage.DeleteFileAsync<Course>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourse([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            try
            {
                var command = new DeleteCourseCommand(id);
                await _mediator.Send(command);
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