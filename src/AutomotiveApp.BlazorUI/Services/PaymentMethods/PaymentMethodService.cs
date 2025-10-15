using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Application.PaymentMethods;
using AutomotiveApp.Shared.Response;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly HttpClient _http;
    public PaymentMethodService(HttpClient http) => _http = http;

    public async Task<IEnumerable<PaymentMethodReadDto>> GetAllAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<IEnumerable<PaymentMethodReadDto>>("api/PaymentMethods", ct)
           ?? Enumerable.Empty<PaymentMethodReadDto>();

    public async Task<Guid?> CreateAsync(PaymentMethodCreateDto dto, CancellationToken ct = default)
    {
        using var resp = await _http.PostAsJsonAsync("api/PaymentMethods/json", dto, ct);
        if (!resp.IsSuccessStatusCode) return null;
        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<PaymentMethodReadDto>>(cancellationToken: ct);
        return body?.Data?.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, PaymentMethodUpdateDto dto, CancellationToken ct = default)
        => (await _http.PutAsJsonAsync($"api/PaymentMethods/{id}/json", dto, ct)).IsSuccessStatusCode;

    public async Task<Guid?> CreateMultipartAsync(string name, bool status, IBrowserFile file, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(name), "Name");
        content.Add(new StringContent(status.ToString()), "Status");

        var stream = file.OpenReadStream(long.MaxValue);
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "FileImageName", file.Name);

        using var resp = await _http.PostAsync("api/PaymentMethods", content, ct);
        if (!resp.IsSuccessStatusCode) return null;

        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<PaymentMethodReadDto>>(cancellationToken: ct);
        return body?.Data?.Id;
    }

    public async Task<bool> UpdateMultipartAsync(Guid id, string name, bool status, IBrowserFile? file, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(name), "Name");
        content.Add(new StringContent(status.ToString()), "Status");

        if (file is not null)
        {
            var stream = file.OpenReadStream(long.MaxValue);
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "FileImageName", file.Name);
        }

        using var resp = await _http.PutAsync($"api/PaymentMethods/{id}", content, ct);
        return resp.IsSuccessStatusCode;
    }
}
