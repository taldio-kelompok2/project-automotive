using AutoMapper;
using AutomotiveApp.Application.Features.CourseCategories.Commands;
using AutomotiveApp.Application.Features.CourseCategories.Queries;
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

    public class CourseCategoryController(IMediator mediator, IMapper mapper, IFileStorage ImageStorage) : BaseApiController(mediator)
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseCategoryQueryDto>>> GetCourseCategories()
        {
            var response = new ApiResponse<IEnumerable<CourseCategoryQueryDto>>();

            try
            {
                var query = new GetCourseCategories();
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
        public async Task<ActionResult<CourseCategoryQueryDto>> GetCourseCategoryById([FromRoute] Guid id, [FromServices] IValidator<GetCourseCategoryById> validator)
        {
            var query = new GetCourseCategoryById(id);
            var validation = await validator.ValidateAsync(query);
            var response = new ApiResponse<CourseCategoryQueryDto>();

            if (!validation.IsValid) return HandleValidationFailure<CourseCategoryQueryDto>(validation);

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
        public async Task<ActionResult<PaginatedResult<CourseCategoryQueryDto>>> GetPagedCourseCategories(
            [FromQuery] int page = 1,
            [FromQuery] int itemTaken = 6
            )
        {
            var response = new ApiResponse<PaginatedResult<CourseCategoryQueryDto>>();

            try
            {
                var query = new GetCourseCategoriesPaged(page, itemTaken);
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
        public async Task<ActionResult<CourseCategoryQueryDto>> AddCourseCategory(
        [FromForm] CourseCategoryCreateRequest request,
        [FromServices] IValidator<CourseCategoryCreateRequest> validator)
        {
            var response = new ApiResponse<CourseCategoryQueryDto>();
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CourseCategoryQueryDto>(validation);
            string? imageFileName = null;
            var dto = mapper.Map<CourseCategoryCommandDto>(request);
            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{request.Id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.SaveFileAsync<CourseCategory>(request.Image.OpenReadStream(), imageFileName);
                }

                dto.ImageFileName = imageFileName;
                var command = new AddCourseCategoryCommand(dto);
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
                    await ImageStorage.DeleteFileAsync<CourseCategory>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CourseCategoryQueryDto>> EditCourseCategory([FromRoute] Guid id,
        [FromForm] CourseCategoryEditRequest request,
        IValidator<CourseCategoryEditRequest> validator)
        {
            var response = new ApiResponse<CourseCategoryQueryDto>();
            request.Id = id;
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CourseCategoryQueryDto>(validation);

            string? imageFileName = null;
            var dto = mapper.Map<CourseCategoryEditDto>(request);
            var command = new EditCourseCategoryCommand(dto);

            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.ReplaceFileAsync<CourseCategory>(imageFileName, request.Image.OpenReadStream());
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
                    await ImageStorage.DeleteFileAsync<CourseCategory>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourseCategory([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            try
            {
                var command = new DeleteCourseCategoryCommand(id);
                await _mediator.Send(command);
                response.Data = $"Course {id} is successfully deleted.";

                response.Success = true;
                response.StatusCode = HttpCode.OK;

                return Ok(response);
            }
            catch (NotFoundException<CourseCategory> ex)
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