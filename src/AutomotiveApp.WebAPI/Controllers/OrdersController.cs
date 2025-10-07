using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Domain.Interface;           // IRepository<>
using AutomotiveApp.Infrastructure.Data;        // AppDbContext
using AutomotiveApp.Application.Orders;         // DTOs

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repo;
        private readonly AppDbContext _db;

        public OrdersController(IRepository<Order> repo, AppDbContext db)
        {
            _repo = repo;
            _db   = db;
        }

        // GET /api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new OrderReadDto
            {
                Id = x.Id,
                TotalPrice = x.TotalPrice,
                Status = x.Status,
                UserId = x.UserId,
                PaymentMethodId = x.PaymentMethodId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
            return Ok(dto);
        }

        // GET /api/orders/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderReadDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new OrderReadDto
            {
                Id = x.Id,
                TotalPrice = x.TotalPrice,
                Status = x.Status,
                UserId = x.UserId,
                PaymentMethodId = x.PaymentMethodId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        // POST /api/orders
        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> Create([FromBody] OrderCreateDto input)
        {
            // Validasi foreign keys cepat
            var userExists = await _db.Users.AnyAsync(u => u.Id == input.UserId);
            if (!userExists) return BadRequest("UserId tidak valid.");

            var pmExists = await _db.PaymentMethods.AnyAsync(p => p.Id == input.PaymentMethodId);
            if (!pmExists) return BadRequest("PaymentMethodId tidak valid.");

            var entity = new Order
            {
                UserId = input.UserId,
                PaymentMethodId = input.PaymentMethodId,
                Status = input.Status,
                TotalPrice = 0,  
            };

            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync();

            var dto = new OrderReadDto
            {
                Id = entity.Id,
                TotalPrice = entity.TotalPrice,
                Status = entity.Status,
                UserId = entity.UserId,
                PaymentMethodId = entity.PaymentMethodId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        // PUT /api/orders/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderUpdateDto input)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var pmExists = await _db.PaymentMethods.AnyAsync(p => p.Id == input.PaymentMethodId);
            if (!pmExists) return BadRequest("PaymentMethodId tidak valid.");

            entity.PaymentMethodId = input.PaymentMethodId;
            entity.Status = input.Status;

            // UpdatedAt via BaseRepository.Update -> MarkUpdated()
            _repo.Update(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/orders/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _repo.Delete(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
