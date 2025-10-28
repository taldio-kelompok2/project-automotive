using AutomotiveApp.Application.Features.Users.Queries;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(IMediator _mediator) : BaseApiController(_mediator)
    {
        [HttpGet("overview")]
        public async Task<ActionResult<ApiResponse<DashboardDto>>> GetOverview(CancellationToken ct)
        {
            var response = new ApiResponse<DashboardDto>();

            try
            {
                var result = await _mediator.Send(new GetDashboard(), ct);
                response.Success = true;
                response.Data = result;
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<ApiResponse<List<DashboardTransactionDto>>>> GetDashboardTransactions(CancellationToken ct = default)
        {
            var response = new ApiResponse<List<DashboardTransactionDto>>();

            try
            {
                var query = new GetAllDashboardTransactions();

                var result = await _mediator.Send(query, ct);
                response.Success = true;
                response.Data = result;
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors = [ex.Message];
                return StatusCode(500, response);
            }
        }

        //[HttpGet("transactions")]
        //public async Task<ActionResult<ApiResponse<List<DashboardUserDto>>>> GetDashboardTransactionsPaged(
        //    [FromQuery] int Page = 1,
        //    [FromQuery] int PageSize = 6,
        //    CancellationToken ct = default)
        //{
        //    var response = new ApiResponse<List<DashboardUserDto>>();

        //    try
        //    {
        //        var query = new GetDashboardTransactionsPaged(Page, PageSize);

        //        var result = await _mediator.Send(query, ct);
        //        response.Success = true;
        //        response.Data = result;
        //        response.StatusCode = HttpStatusCode.OK;

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Errors = [ex.Message];
        //        return StatusCode(500, response);
        //    }
        //}
    }
}
