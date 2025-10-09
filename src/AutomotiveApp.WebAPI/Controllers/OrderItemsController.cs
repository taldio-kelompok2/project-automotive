using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Shared.Dtos.OrderItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IRepository<OrderItem> _repo;
        private readonly AppDbContext _db;

        public OrderItemsController(IRepository<OrderItem> repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        // GET /api/orderitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemReadDto>>> GetAll(CancellationToken ct)
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new OrderItemReadDto
            {
                Id = x.Id,
                Price = x.Price,
                OrderId = x.OrderId,
                SessionId = x.SessionId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
            return Ok(dto);
        }

        // GET /api/orderitems/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderItemReadDto>> GetById(Guid id, CancellationToken ct)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new OrderItemReadDto
            {
                Id = x.Id,
                Price = x.Price,
                OrderId = x.OrderId,
                SessionId = x.SessionId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        // POST /api/orderitems
        [HttpPost]
        public async Task<ActionResult<OrderItemReadDto>> Create([FromBody] OrderItemCreateDto input, CancellationToken ct)
        {
            var orderExists = await _db.Orders.AnyAsync(o => o.Id == input.OrderId, ct);
            if (!orderExists) return BadRequest("OrderId tidak valid.");

            var sessionExists = await _db.CourseSessions.AnyAsync(s => s.Id == input.SessionId, ct);
            if (!sessionExists) return BadRequest("SessionId tidak valid.");

            var entity = new OrderItem
            {
                Price = input.Price,
                OrderId = input.OrderId,
                SessionId = input.SessionId
            };

            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync(ct);

            var dto = new OrderItemReadDto
            {
                Id = entity.Id,
                Price = entity.Price,
                OrderId = entity.OrderId,
                SessionId = entity.SessionId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        // PUT /api/orderitems/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderItemUpdateDto input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            // (opsional) validasi session
            var sessionExists = await _db.CourseSessions.AnyAsync(s => s.Id == input.SessionId, ct);
            if (!sessionExists) return BadRequest("SessionId tidak valid.");

            entity.Price = input.Price;
            entity.SessionId = input.SessionId;

            _repo.Update(entity);
            await _db.SaveChangesAsync(ct);

            return NoContent();
        }

        // DELETE /api/orderitems/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _repo.Delete(entity);
            await _db.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}
