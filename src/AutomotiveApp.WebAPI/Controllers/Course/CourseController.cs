using AutoMapper;
using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Dto;
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
        public async Task<ActionResult<CourseQueryDto>> GetCourseById([FromRoute] Guid id, [FromServices] IValidator<GetCourseById> validator)
        {
            var query = new GetCourseById(id);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseQueryDto>(validation);

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
        public async Task<ActionResult<CourseQueryDto>> AddCourse([FromForm] CourseCreateRequest req)
        {
            var response = new ApiResponse<CourseQueryDto>();
            string? imageFileName = null;
            try
            {
                if (req.Image != null)
                {
                    imageFileName = $"{req.Id}{Path.GetExtension(req.Image.FileName)}";
                    await ImageStorage.SaveFileAsync<Course>(req.Image.OpenReadStream(), imageFileName);
                }

                var dto = mapper.Map<CourseCommandDto>(req);
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


    }

}