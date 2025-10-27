using System.Net;

namespace AutomotiveApp.Shared.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public ApiResponse()
        {
            Success = false;
            StatusCode = HttpStatusCode.InternalServerError;
            Data = default;
            Errors = [];
        }
    }
}