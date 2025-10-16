using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Application.Invoices;
using AutomotiveApp.Domain.Entities.Invoices;
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
            _db = db;
        }

        // GET: /api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new InvoiceReadDto
            {
                Id = x.Id,
                TotalPrice = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceCode = x.InvoiceCode,
                OrderId = x.OrderId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
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
                Id = x.Id,
                TotalPrice = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceCode = x.InvoiceCode,
                OrderId = x.OrderId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
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
                Id = x.Id,
                TotalPrice = x.TotalPrice,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceCode = x.InvoiceCode,
                OrderId = x.OrderId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        // POST: /api/invoices/generate
        [HttpPost("create-form")]
        [ProducesResponseType(typeof(InvoiceReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InvoiceReadDto>> CreateForm([FromForm] InvoiceCreateFormDto form)
        {
            var order = await _db.Orders.Include(o => o.OrderItems)
                                        .FirstOrDefaultAsync(o => o.Id == form.OrderId);
            if (order == null) return BadRequest("Order Id Not Found");

            var totalPrice = order.OrderItems.Sum(oi => (long)oi.Price);
            var lastNumber = await _db.Invoices.MaxAsync(i => (int?)i.InvoiceNumber) ?? 0;

            var invoice = new Invoice
            {
                OrderId = order.Id,
                TotalPrice = totalPrice,
                InvoiceNumber = lastNumber + 1
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            var dto = new InvoiceReadDto
            {
                Id = invoice.Id,
                TotalPrice = invoice.TotalPrice,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceCode = invoice.InvoiceCode,
                OrderId = invoice.OrderId,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, dto);
        }

        // PUT: /api/invoices/{id}/update-form
        [HttpPut("{id:guid}/update-form")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateForm(Guid id, [FromForm] InvoiceUpdateFormDto form)
        {
            var inv = await _repo.GetByIdAsync(id);
            if (inv == null) return NotFound();

            // contoh: izinkan update nomor & total (kalau kebijakan kamu mengizinkan)
            inv.InvoiceNumber = form.InvoiceNumber;
            inv.TotalPrice = form.TotalPrice;

            _repo.Update(inv);
            await _db.SaveChangesAsync();
            return NoContent();
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

        // GET: /api/invoices/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> Search(
            [FromQuery] int? invoiceNumber,
            [FromQuery] Guid? orderId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _db.Invoices.AsQueryable();

            if (invoiceNumber.HasValue)
                query = query.Where(i => i.InvoiceNumber == invoiceNumber.Value);

            if (orderId.HasValue)
                query = query.Where(i => i.OrderId == orderId.Value);

            if (from.HasValue)
                query = query.Where(i => i.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(i => i.CreatedAt <= to.Value);

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InvoiceReadDto
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    TotalPrice = i.TotalPrice,
                    OrderId = i.OrderId,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt,
                    InvoiceCode = i.InvoiceCode
                })
                .ToListAsync();

            return Ok(new
            {
                totalItems,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                items
            });
        }

        // GET: /api/invoices/{id}/details
        [HttpGet("{id:guid}/details")]
        public async Task<ActionResult<InvoiceDetailsDto>> GetDetailsById(Guid id)
        {
            var inv = await _db.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            if (inv is null) return NotFound();

            var items = await (
                from oi  in _db.OrderItems.AsNoTracking()
                join cs  in _db.CourseSessions.AsNoTracking()   on oi.SessionId  equals cs.Id
                join c   in _db.Courses.AsNoTracking()          on cs.CourseId   equals c.Id
                join cat in _db.CourseCategories.AsNoTracking() on c.CategoryId  equals cat.Id
                where oi.OrderId == inv.OrderId
                select new InvoiceItemDto
                {
                    CourseName = c.Name,
                    Type       = cat.Name,
                    Schedule   = cs.Date,
                    Price      = oi.Price
                }
            ).ToListAsync();

            var dto = new InvoiceDetailsDto
            {
                Id          = inv.Id,
                InvoiceCode = inv.InvoiceCode,
                CreatedAt   = inv.CreatedAt,
                TotalPrice  = inv.TotalPrice > 0 ? inv.TotalPrice : (long)items.Sum(x => x.Price),
                Items       = items
            };

            return Ok(dto);
        }

    }
}
