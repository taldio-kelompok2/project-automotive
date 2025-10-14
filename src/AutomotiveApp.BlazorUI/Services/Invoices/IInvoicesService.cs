using AutomotiveApp.Application.Invoices;

namespace AutomotiveApp.BlazorUI.Services.Invoices;

public interface IInvoiceService
{
    Task<List<InvoiceReadDto>> GetAllAsync();
    Task<InvoiceReadDto?> GetByIdAsync(Guid id);
    Task<List<InvoiceReadDto>> GetByOrderAsync(Guid orderId);
    Task<bool> DeleteAsync(Guid id);

    Task<List<InvoiceReadDto>> SearchAsync(string? keyword = null, int? page = null, int? itemTaken = null);

    Task<InvoiceReadDto?> CreateFormAsync(InvoiceCreateFormDto dto);
    Task<bool> UpdateFormAsync(Guid id, InvoiceUpdateFormDto dto);
}