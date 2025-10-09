using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Shared.Dtos.Carts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CartsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetAll()
        {
            var carts = await _db.Carts.AsNoTracking().ToListAsync();

            var result = carts.Select(c => new CartReadDto
            {
                Id = c.Id,
                TotalPrice = c.TotalPrice,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartReadDto>> GetById(Guid id)
        {
            var cart = await _db.Carts.FindAsync(id);
            if (cart == null) return NotFound("Cart Not Found");

            var dto = new CartReadDto
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CartReadDto>> Create([FromForm] CartCreateDto input)
        {
            var cart = new Cart
            {
                UserId = input.UserId,
                TotalPrice = input.TotalPrice,
            };

            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();

            var dto = new CartReadDto
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, dto);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CartReadDto>> Update(Guid id, [FromForm] CartUpdateDto input)
        {
            var existing = await _db.Carts.FindAsync(id);
            if (existing == null) return NotFound("Cart Not Found");

            existing.TotalPrice = input.TotalPrice;

            _db.Entry(existing).Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var dto = new CartReadDto
            {
                Id = existing.Id,
                TotalPrice = existing.TotalPrice,
                UserId = existing.UserId,
                CreatedAt = existing.CreatedAt,
                UpdatedAt = existing.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cart = await _db.Carts.FindAsync(id);
            if (cart == null) return NotFound("Cart Not Found");

            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}