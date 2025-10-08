using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Shared.Dtos.CartItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomotiveApp.Domain.Entities.Courses.Cart;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CartItemsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetAll()
        {
            var items = await _db.CartItems.AsNoTracking().ToListAsync();
            var dto = items.Select(ci => new CartItemReadDto
            {
                Id = ci.Id,
                CartId = ci.CartId,
                SessionId = ci.SessionId,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt
            });
            return Ok(dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartItemReadDto>> GetById(Guid id)
        {
            var ci = await _db.CartItems.FindAsync(id);
            if (ci == null) return NotFound("Cart Item Not Found");

            var dto = new CartItemReadDto
            {
                Id = ci.Id,
                CartId = ci.CartId,
                SessionId = ci.SessionId,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt
            };
            return Ok(dto);
        }

        [HttpGet("by-cart/{cartId:guid}")]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetByCart(Guid cartId)
        {
            var list = await _db.CartItems
                .AsNoTracking()
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            var dto = list.Select(ci => new CartItemReadDto
            {
                Id = ci.Id,
                CartId = ci.CartId,
                SessionId = ci.SessionId,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt
            });

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemReadDto>> Create([FromForm] CartItemCreateDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // validasi foreign key cart & Session 
            var cartExists = await _db.Carts.AnyAsync(c => c.Id == input.CartId);
            if (!cartExists) return BadRequest("Cart Id Not Found");

            var entity = new CartItem
            {
                CartId = input.CartId,
                SessionId = input.SessionId,
            };

            _db.CartItems.Add(entity);
            await _db.SaveChangesAsync();

            var dto = new CartItemReadDto
            {
                Id = entity.Id,
                CartId = entity.CartId,
                SessionId = entity.SessionId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CartItemReadDto>> Update(Guid id, [FromForm] CartItemUpdateDto input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = await _db.CartItems.FindAsync(id);
            if (entity == null) return NotFound("Cart Item Not Found");

            entity.SessionId = input.SessionId;
            entity.MarkUpdated(); 

            await _db.SaveChangesAsync();

            var dto = new CartItemReadDto
            {
                Id = entity.Id,
                CartId = entity.CartId,
                SessionId = entity.SessionId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _db.CartItems.FindAsync(id);
            if (entity == null) return NotFound("Cart Item Not Found");

            _db.CartItems.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}