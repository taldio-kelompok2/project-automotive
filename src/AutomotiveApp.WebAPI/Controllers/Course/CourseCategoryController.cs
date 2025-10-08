using AutomotiveApp.Application.Features.CourseCategories.Queries;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers.Courses
{
    [ApiController]
    [Route("api/[controller]")]

    public class CourseCategoryController(IMediator mediator) : BaseApiController(mediator)
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
        public async Task<ActionResult<CourseCategoryQueryDto>> GetCourseById([FromRoute] Guid id, [FromServices] IValidator<GetCourseCategoryById> validator)
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
    }

}