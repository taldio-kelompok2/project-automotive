using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Shared.Dtos;
using MediatR;
using AutomotiveApp.Base.Entities;
using AutomotiveApp.Application.Interfaces.Utils;

namespace AutomotiveApp.WebAPI.Controllers
{
    public abstract class BaseApiController(IMediator mediator) : ControllerBase
    {
        protected readonly IMediator _mediator = mediator;

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