using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Shared.Dtos;
using MediatR;
using System.Net;
using System.Linq;

namespace AutomotiveApp.WebAPI.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        private const string BearerPrefix = "Bearer ";
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
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
            };

            return BadRequest(errorResponse);
        }

        // protected string? ExtractAccessTokenFromHeader()
        // {
        //     try
        //     {
        //         var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

        //         if (string.IsNullOrEmpty(authorizationHeader))
        //             return null;

        //         if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //         {
        //             return authorizationHeader.Substring("Bearer ".Length).Trim();
        //         }

        //         return authorizationHeader;
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }

        protected string? ExtractAccessTokenFromHeader([FromHeader(Name = "Authorization")] string? authorizationHeader)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authorizationHeader))
                    return null;

                if (authorizationHeader != null &&
    authorizationHeader.Length > BearerPrefix.Length &&
    authorizationHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    // Extract the token safely
                    return authorizationHeader[BearerPrefix.Length..].Trim();
                }

                return authorizationHeader;
            }
            catch
            {
                return null;
            }
        }

        protected string? ExtractAccessTokenFromHeader()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault(); // NOSONAR
                return ExtractAccessTokenFromHeader(authorizationHeader);
            }
            catch
            {
                return null;
            }
        }

    }

}