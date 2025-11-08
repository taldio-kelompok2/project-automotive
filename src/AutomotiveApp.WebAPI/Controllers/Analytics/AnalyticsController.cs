using AutomotiveApp.Application.Features.Analytics.Queries;
using AutomotiveApp.Shared.Dtos.Analytics;
using AutomotiveApp.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AutomotiveApp.WebAPI.Controllers.Analytics
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController(IMediator _mediator) : BaseApiController(_mediator)
    {

        [HttpGet("overview")]
        public async Task<ActionResult<ApiResponse<AnalyticsOverviewDto>>> GetOverview(CancellationToken ct = default)
        {
            var response = new ApiResponse<AnalyticsOverviewDto>();
            try
            {
                var result = await _mediator!.Send(new GetCourseAnalyticsOverview(), ct);
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
        
    }
}
