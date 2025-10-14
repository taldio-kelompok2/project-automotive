using System.Net.Http.Json;
using System.Web;
using AutomotiveApp.Application.Invoices;

namespace AutomotiveApp.BlazorUI.Services.Invoices;

public class InvoiceService(HttpClient http) : IInvoiceService
{
    public async Task<List<InvoiceReadDto>> GetAllAsync()
        => await http.GetFromJsonAsync<List<InvoiceReadDto>>("api/Invoices") ?? new();

    public async Task<InvoiceReadDto?> GetByIdAsync(Guid id)
        => await http.GetFromJsonAsync<InvoiceReadDto>($"api/Invoices/{id}");

    public async Task<List<InvoiceReadDto>> GetByOrderAsync(Guid orderId)
        => await http.GetFromJsonAsync<List<InvoiceReadDto>>($"api/Invoices/by-order/{orderId}") ?? new();

    public async Task<bool> DeleteAsync(Guid id)
    {
        var res = await http.DeleteAsync($"api/Invoices/{id}");
        return res.IsSuccessStatusCode;
    }

    public async Task<List<InvoiceReadDto>> SearchAsync(string? keyword = null, int? page = null, int? itemTaken = null)
    {
        var qb = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(keyword)) qb["q"] = keyword; // ganti "q" jika API pakai nama lain
        if (page is not null) qb["page"] = page.Value.ToString();
        if (itemTaken is not null) qb["itemTaken"] = itemTaken.Value.ToString();

        var url = $"api/Invoices/search?{qb}";
        return await http.GetFromJsonAsync<List<InvoiceReadDto>>(url) ?? new();
    }

    public async Task<InvoiceReadDto?> CreateFormAsync(InvoiceCreateFormDto dto)
    {
        var res = await http.PostAsJsonAsync("api/Invoices/create-form", dto);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<InvoiceReadDto>();
    }

    public async Task<bool> UpdateFormAsync(Guid id, InvoiceUpdateFormDto dto)
    {
        var res = await http.PutAsJsonAsync($"api/Invoices/{id}/update-form", dto);
        if (res.IsSuccessStatusCode) return true;

        return false;
    }
}