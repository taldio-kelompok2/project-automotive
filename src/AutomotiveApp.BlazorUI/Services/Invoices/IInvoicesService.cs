using AutomotiveApp.Application.Invoices;

namespace AutomotiveApp.BlazorUI.Services.Invoices;

public interface IInvoiceService
{
    Task<List<InvoiceReadDto>> GetAllAsync(CancellationToken ct = default);

    Task<InvoiceReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<InvoiceReadDto?> GetByOrderAsync(Guid orderId, CancellationToken ct = default);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    public record InvoiceSearchResult(int totalItems, int page, int pageSize, int totalPages, List<InvoiceReadDto> items);
    Task<InvoiceSearchResult?> SearchAsync(
        int? invoiceNumber = null,
        Guid? orderId = null,
        DateTime? from = null,
        DateTime? to = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default);

    Task<InvoiceReadDto?> CreateFormAsync(InvoiceCreateFormDto dto, CancellationToken ct = default);

    Task<bool> UpdateFormAsync(Guid id, InvoiceUpdateFormDto dto, CancellationToken ct = default);

    Task<List<InvoiceReadDto>> GetMyInvoicesAsync(Guid? orderId = null, int? invoiceNumber = null, CancellationToken ct = default);

    Task<InvoiceDetailsDto?> GetDetailsAsync(Guid id, CancellationToken ct = default);

    string GetPdfUrl(Guid id);
}
