using AutomotiveApp.Application.PaymentMethods;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading;

public interface IPaymentMethodService
{
    Task<IEnumerable<PaymentMethodReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<Guid?> CreateAsync(PaymentMethodCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, PaymentMethodUpdateDto dto, CancellationToken ct = default);

    Task<Guid?> CreateMultipartAsync(string name, bool status, IBrowserFile file, CancellationToken ct = default);
    Task<bool>   UpdateMultipartAsync(Guid id, string name, bool status, IBrowserFile? file, CancellationToken ct = default);
}