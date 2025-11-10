using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Features.Users.Queries;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Dto.Courses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AutomotiveApp.WebAPI.Controllers
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
                var result = await Mediator.Send(new GetDashboard(), ct);
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

                var result = await Mediator.Send(query, ct);
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

        [HttpGet("statistics/course")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseStatisticDto>>>> GetStatistics(
            [FromQuery] int? year,
            CancellationToken ct = default)
        {
            var response = new ApiResponse<IEnumerable<CourseStatisticDto>>();
            var query = new GetCourseStatistic(year);
            var result = await Mediator.Send(query, ct);

            response.Success = true;
            response.Data = result;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
