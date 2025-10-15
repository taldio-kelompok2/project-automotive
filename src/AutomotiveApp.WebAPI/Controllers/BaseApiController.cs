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

        protected string? ExtractAccessTokenFromHeader()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authorizationHeader))
                    return null;

                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return authorizationHeader.Substring("Bearer ".Length).Trim();
                }

                return authorizationHeader;
            }
            catch
            {
                return null;
            }
        }
    }
}