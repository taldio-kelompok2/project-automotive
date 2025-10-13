using System.Net;
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

            var query = new GetCourseCategories();
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseCategoryQueryDto>> GetCourseCategoryById([FromRoute] Guid id, [FromServices] IValidator<GetCourseCategoryById> validator)
        {
            var query = new GetCourseCategoryById(id);
            var response = new ApiResponse<CourseCategoryQueryDto>();
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginatedResult<CourseCategoryQueryDto>>> GetPagedCourseCategories(
            [FromQuery] int page = 1,
            [FromQuery] int itemTaken = 6
            )
        {
            var response = new ApiResponse<PaginatedResult<CourseCategoryQueryDto>>();

            var query = new GetCourseCategoriesPaged(page, itemTaken);
            var result = await Mediator.Send(query);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CourseCategoryQueryDto>> AddCourseCategory(
        [FromForm] CourseCategoryCreateRequest request,
        [FromServices] IValidator<CourseCategoryCreateRequest> validator)
        {
            var response = new ApiResponse<CourseCategoryQueryDto>();
            await validator.ValidateAndThrowAsync(request);
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
                    await ImageStorage.DeleteFileAsync<CourseCategory>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
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
            await validator.ValidateAndThrowAsync(request);
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
                    await ImageStorage.DeleteFileAsync<CourseCategory>(imageFileName);
                }
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourseCategory([FromRoute] Guid id)
        {
            var response = new ApiResponse<string>();
            var command = new DeleteCourseCategoryCommand(id);
            await Mediator.Send(command);
            response.Data = $"Course {id} is successfully deleted.";

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }


    }

}