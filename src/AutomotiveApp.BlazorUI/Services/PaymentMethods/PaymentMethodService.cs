using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Application.PaymentMethods;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Shared.Config;

namespace AutomotiveApp.BlazorUI.Services.PaymentMethods
{
    public sealed class PaymentMethodService : IPaymentMethodService
    {
        private readonly HttpClient _http;
        private const string BasePath = "api/PaymentMethods";

        public PaymentMethodService(IHttpClientFactory httpClientFactory)
            => _http = httpClientFactory.CreateClient("ServerAPI");

        public async Task<IEnumerable<PaymentMethodReadDto>> GetAllAsync(CancellationToken ct = default)
            => await _http.GetFromJsonAsync<IEnumerable<PaymentMethodReadDto>>(BasePath, ct)
                ?? Enumerable.Empty<PaymentMethodReadDto>();

        public async Task<Guid?> CreateAsync(PaymentMethodCreateDto dto, CancellationToken ct = default)
        {
            using var resp = await _http.PostAsJsonAsync($"{BasePath}/json", dto, ct);
            if (!resp.IsSuccessStatusCode) return null;

            var body = await resp.Content.ReadFromJsonAsync<ApiResponse<PaymentMethodReadDto>>(cancellationToken: ct);
            return body?.Data?.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, PaymentMethodUpdateDto dto, CancellationToken ct = default)
        {
            using var resp = await _http.PutAsJsonAsync($"{BasePath}/{id}/json", dto, ct);
            return resp.IsSuccessStatusCode;
        }

        public async Task<Guid?> CreateMultipartAsync(string name, bool status, IBrowserFile file, CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(name), "Name");
            content.Add(new StringContent(status.ToString()), "Status");

            await using var stream = file.OpenReadStream(FileUploadConfig.MaxFileSize);
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "FileImageName", file.Name);

            using var resp = await _http.PostAsync(BasePath, content, ct);
            if (!resp.IsSuccessStatusCode) return null;

            var body = await resp.Content.ReadFromJsonAsync<ApiResponse<PaymentMethodReadDto>>(cancellationToken: ct);
            return body?.Data?.Id;
        }

        public async Task<bool> UpdateMultipartAsync(Guid id, string name, bool status, IBrowserFile? file, CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(name), "Name");
            content.Add(new StringContent(status.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Status");

            if (file is not null)
            {
                var stream = file.OpenReadStream(FileUploadConfig.MaxFileSize, ct);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "FileImageName", file.Name);
            }

            using var resp = await _http.PutAsync($"{BasePath}/{id}", content, ct);
            var responseText = await resp.Content.ReadAsStringAsync(ct);

            Console.WriteLine($"Response: {resp.StatusCode}");
            Console.WriteLine(responseText);

            return resp.IsSuccessStatusCode;
        }

    }
}
