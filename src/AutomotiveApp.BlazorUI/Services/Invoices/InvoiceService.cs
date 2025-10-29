using System.Net.Http.Json;
using System.Web;
using AutomotiveApp.Application.Invoices;

namespace AutomotiveApp.BlazorUI.Services.Invoices;

public class InvoiceService(IHttpClientFactory httpFactory) : IInvoiceService
{
    private readonly HttpClient _http = httpFactory.CreateClient("ServerAPI");
    private const string BaseEndpoint = "api/Invoices";

    public async Task<List<InvoiceReadDto>> GetAllAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<InvoiceReadDto>>($"{BaseEndpoint}", ct) ?? new();

    public async Task<InvoiceReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<InvoiceReadDto>($"{BaseEndpoint}/{id}", ct);

    // controller returns single item
    public async Task<InvoiceReadDto?> GetByOrderAsync(Guid orderId, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<InvoiceReadDto>($"{BaseEndpoint}/by-order/{orderId}", ct);

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => (await _http.DeleteAsync($"{BaseEndpoint}/{id}", ct)).IsSuccessStatusCode;

    // search 
    public async Task<IInvoiceService.InvoiceSearchResult?> SearchAsync(
        int? invoiceNumber = null,
        Guid? orderId = null,
        DateTime? from = null,
        DateTime? to = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        var qb = HttpUtility.ParseQueryString(string.Empty);
        if (invoiceNumber.HasValue) qb["invoiceNumber"] = invoiceNumber.Value.ToString();
        if (orderId.HasValue) qb["orderId"] = orderId.Value.ToString();
        if (from.HasValue) qb["from"] = from.Value.ToString("o");
        if (to.HasValue) qb["to"] = to.Value.ToString("o");
        qb["page"] = page.ToString();
        qb["pageSize"] = pageSize.ToString();

        var url = $"{BaseEndpoint}/search?{qb}";
        return await _http.GetFromJsonAsync<IInvoiceService.InvoiceSearchResult>(url, ct);
    }

    // controller expects [FromForm] => send multipart/form-data
    public async Task<InvoiceReadDto?> CreateFormAsync(InvoiceCreateFormDto dto, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent
        {
            { new StringContent(dto.OrderId.ToString()), nameof(InvoiceCreateFormDto.OrderId) }
        };
        var res = await _http.PostAsync($"{BaseEndpoint}/create-form", form, ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<InvoiceReadDto>(cancellationToken: ct);
    }

    // controller expects [FromForm] => send multipart/form-data
    public async Task<bool> UpdateFormAsync(Guid id, InvoiceUpdateFormDto dto, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent
        {
            { new StringContent(dto.InvoiceNumber.ToString()), nameof(InvoiceUpdateFormDto.InvoiceNumber) },
            { new StringContent(dto.TotalPrice.ToString()), nameof(InvoiceUpdateFormDto.TotalPrice) }
        };
        var res = await _http.PutAsync($"{BaseEndpoint}/{id}/update-form", form, ct);
        return res.IsSuccessStatusCode;
    }

    // me
    public async Task<List<InvoiceReadDto>> GetMyInvoicesAsync(
        Guid? orderId = null,
        int? invoiceNumber = null,
        CancellationToken ct = default)
    {
        var qb = HttpUtility.ParseQueryString(string.Empty);
        if (orderId.HasValue) qb["orderId"] = orderId.Value.ToString();
        if (invoiceNumber.HasValue) qb["invoiceNumber"] = invoiceNumber.Value.ToString();

        var url = $"{BaseEndpoint}/me?{qb}";
        return await _http.GetFromJsonAsync<List<InvoiceReadDto>>(url, ct) ?? new();
    }

    public async Task<InvoiceDetailsDto?> GetDetailsAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<InvoiceDetailsDto>($"{BaseEndpoint}/{id}/details", ct);
}
