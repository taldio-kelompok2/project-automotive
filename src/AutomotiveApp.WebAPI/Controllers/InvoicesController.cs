using AutomotiveApp.Application.Invoices;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Domain.Interface;          
using AutomotiveApp.Infrastructure.Data;     
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IRepository<Invoice> _repo;
        private readonly AppDbContext _db;

        public InvoicesController(IRepository<Invoice> repo, AppDbContext db)
        {
            _repo = repo;
            _db   = db;
        }

        // GET: /api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new InvoiceReadDto
            {
                Id            = x.Id,
                TotalPrice    = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                OrderId       = x.OrderId,
                CreatedAt     = x.CreatedAt,
                UpdatedAt     = x.UpdatedAt
            });
            return Ok(dto);
        }

        // GET: /api/invoices/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<InvoiceReadDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new InvoiceReadDto
            {
                Id            = x.Id,
                TotalPrice    = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                OrderId       = x.OrderId,
                CreatedAt     = x.CreatedAt,
                UpdatedAt     = x.UpdatedAt
            });
        }

        // GET: /api/invoices/by-order/{orderId}
        [HttpGet("by-order/{orderId:guid}")]
        public async Task<ActionResult<InvoiceReadDto>> GetByOrderId(Guid orderId)
        {
            var x = await _db.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);
            if (x == null) return NotFound();

            return Ok(new InvoiceReadDto
            {
                Id            = x.Id,
                TotalPrice    = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                OrderId       = x.OrderId,
                CreatedAt     = x.CreatedAt,
                UpdatedAt     = x.UpdatedAt
            });
        }

        // POST: /api/invoices/generate/{orderId}
        [HttpPost]
        public async Task<ActionResult<InvoiceReadDto>> Create([FromBody] InvoiceCreateDto input)
        {
            // Data Order
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == input.OrderId);

            if (order == null)
                return NotFound($"Order Dengan ID {input.OrderId} Tidak Ada");

            // Hitung Total Harga Dari OrderItem
            var totalPrice = order.OrderItems.Sum(oi => (decimal)oi.Price);

            // Nomor Invoice Terakhir
            var lastInvoiceNumber = await _db.Invoices.MaxAsync(i => (int?)i.InvoiceNumber) ?? 0;

            // Create Invoice Baru
            var invoice = new Invoice
            {
                OrderId = order.Id,
                TotalPrice = (uint)totalPrice, 
                InvoiceNumber = lastInvoiceNumber + 1
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            var dto = new InvoiceReadDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                TotalPrice = invoice.TotalPrice,
                InvoiceNumber = invoice.InvoiceNumber,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, dto);
        }

        // DELETE: /api/invoices/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var inv = await _repo.GetByIdAsync(id);
            if (inv == null) return NotFound();

            _repo.Delete(inv);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
