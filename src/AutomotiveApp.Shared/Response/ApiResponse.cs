using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.Shared.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public HttpCode StatusCode { get; set; } = HttpCode.OK;
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

}