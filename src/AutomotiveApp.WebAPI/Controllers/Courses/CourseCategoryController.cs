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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.WebAPI.Controllers.Courses
{
    [ApiController]
    [Route("api/[controller]")]

    public class CourseCategoryController(IMediator mediator, IMapper mapper, IFileStorage ImageStorage, ILogger<CourseCategoryController> logger) : BaseApiController(mediator)
    {
        // Logger
        private readonly ILogger<CourseCategoryController> _logger = logger;

        // Helper Tambahan HeroImage
        private string? ResolveHeroUrl(Guid id)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Images", "CourseCategory");
            if (!Directory.Exists(root)) return null;

            var pattern = $"{id}-hero.*";
            var match = Directory.GetFiles(root, pattern).FirstOrDefault();
            if (match is null) return null;

            var fileName = Path.GetFileName(match);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return $"{baseUrl}/images/CourseCategory/{fileName}";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseCategoryQueryDto>>> GetCourseCategories()
        {
            _logger.LogInformation("GET /api/coursecategory - fetching categories");

            var response = new ApiResponse<IEnumerable<CourseCategoryQueryDto>>();
            var query = new GetCourseCategories();
            var result = await Mediator.Send(query);

            // HeroImage
            // foreach (var item in result)
            //     item.HeroImageUrl ??= ResolveHeroUrl(item.Id);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            _logger.LogInformation("GET /api/coursecategory - returned {Count} items", result?.Count() ?? 0);

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseCategoryQueryDto>> GetCourseCategoryById([FromRoute] Guid id, [FromServices] IValidator<GetCourseCategoryById> validator)
        {
            _logger.LogInformation("GET /api/coursecategory/{Id} - start", id);

            var query = new GetCourseCategoryById(id);
            var response = new ApiResponse<CourseCategoryQueryDto>();
            await validator.ValidateAndThrowAsync(query);

            var result = await Mediator.Send(query);

            // HeroImage
            result.HeroImageUrl ??= ResolveHeroUrl(result.Id);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            _logger.LogInformation("GET /api/coursecategory/{Id} - ok", id);

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

            // HeroImage
            foreach (var item in result.Items)
                item.HeroImageUrl ??= ResolveHeroUrl(item.Id);

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = result;

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CourseCategoryQueryDto>> AddCourseCategory(
        [FromForm] CourseCategoryCreateRequest request,
        [FromServices] IValidator<CourseCategoryCreateRequest> validator)
        {
            _logger.LogInformation("POST /api/coursecategory - creating {Name}", request?.Name);

            var response = new ApiResponse<CourseCategoryQueryDto>();
            await validator.ValidateAndThrowAsync(request);
            string? imageFileName = null;
            string? heroFileName = null;
            var dto = mapper.Map<CourseCategoryCommandDto>(request);
            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{request.Id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.SaveFileAsync<CourseCategory>(request.Image.OpenReadStream(), imageFileName);
                }

                if (request.HeroImage != null)
                {
                    heroFileName = $"{request.Id}-hero{Path.GetExtension(request.HeroImage.FileName)}";
                    await ImageStorage.SaveFileAsync<CourseCategory>(request.HeroImage.OpenReadStream(), heroFileName);
                }

                dto.ImageFileName = imageFileName;
                dto.HeroImageFileName = heroFileName;
                var command = new AddCourseCategoryCommand(dto);
                var result = await Mediator.Send(command);
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Data = result;

                _logger.LogInformation("POST /api/coursecategory - created category {CategoryId}", result.Id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST /api/coursecategory failed for {Name}", request?.Name);

                if (imageFileName != null)
                {
                    await ImageStorage.DeleteFileAsync<CourseCategory>(imageFileName);
                }
                if (heroFileName != null)
                {
                    await ImageStorage.DeleteFileAsync<CourseCategory>(heroFileName);
                }
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CourseCategoryQueryDto>> EditCourseCategory([FromRoute] Guid id,
        [FromForm] CourseCategoryEditRequest request,
        IValidator<CourseCategoryEditRequest> validator)
        {
            var response = new ApiResponse<CourseCategoryQueryDto>();
            request.Id = id;
            await validator.ValidateAndThrowAsync(request);
            string? imageFileName = null;
            string? heroFileName = null;
            var dto = mapper.Map<CourseCategoryEditDto>(request);
            var command = new EditCourseCategoryCommand(dto);

            try
            {
                if (request.Image != null)
                {
                    imageFileName = $"{id}{Path.GetExtension(request.Image.FileName)}";
                    await ImageStorage.ReplaceFileAsync<CourseCategory>(imageFileName, request.Image.OpenReadStream());
                }

                if (request.HeroImage != null)
                {
                    heroFileName = $"{id}-hero{Path.GetExtension(request.HeroImage.FileName)}";
                    await ImageStorage.ReplaceFileAsync<CourseCategory>(heroFileName, request.HeroImage.OpenReadStream());
                }

                dto.ImageFilename = imageFileName;
                dto.HeroImageFilename = heroFileName;
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
                if (heroFileName != null)
                {
                    await ImageStorage.DeleteFileAsync<CourseCategory>(heroFileName);
                }
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];

                return BadRequest(response);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
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