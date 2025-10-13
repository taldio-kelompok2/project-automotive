using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Shared.Dtos;
using MediatR;

namespace AutomotiveApp.WebAPI.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IMediator? _mediator;
        public BaseApiController() { }
        protected BaseApiController(IMediator? mediator = null)
        {
            _mediator = mediator;
        }

        protected IMediator Mediator => _mediator
        ?? throw new InvalidOperationException("Mediator not configured for this controller");

        protected ActionResult<T> HandleValidationFailure<T>(ValidationResult validation) where T : IDto
        {
            var errorResponse = new ApiResponse<T>
            {
                Success = false,
                StatusCode = HttpCode.BadRequest,
                Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
            };

            return BadRequest(errorResponse);
        }
    }

}