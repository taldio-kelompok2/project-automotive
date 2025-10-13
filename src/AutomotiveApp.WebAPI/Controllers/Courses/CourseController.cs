using System.Net;
using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Dto.Courses;
using AutomotiveApp.WebAPI.Helper;
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
        public async Task<ActionResult<IEnumerable<CourseQueryDto>>> GetAll()
        {
            var response = new ApiResponse<IEnumerable<CourseQueryDto>>();
            var query = new GetCourses();
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseQueryDetailDto>> GetById([FromRoute] Guid id,
        [FromServices] IValidator<GetCourseById> validator)
        {
            var user = User.GetCurrentUserId();
            var query = new GetCourseById(id, user);
            await validator.ValidateAndThrowAsync(query);
            var response = new ApiResponse<CourseQueryDetailDto>();

            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginatedResult<CourseQueryDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int itemTaken = 6
            )
        {
            var response = new ApiResponse<PaginatedResult<CourseQueryDto>>();
            var query = new GetCoursesPaged(page, itemTaken);
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CourseQueryDto>> Add([FromForm] CourseCreateRequest request,
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
                var result = await Mediator.Send(command);
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
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
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CourseQueryDto>> Edit([FromRoute] Guid id, [FromForm] CourseEditRequest request,
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
                var result = await Mediator.Send(command);
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
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
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            var command = new DeleteCourseCommand(id);
            await Mediator.Send(command);
            response.Data = $"Course {id} is successfully deleted.";

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }

    }
}