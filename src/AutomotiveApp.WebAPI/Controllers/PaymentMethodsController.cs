using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutomotiveApp.Domain.Entities.Payments;          // PaymentMethod (Entity)
// using AutomotiveApp.Domain.Interface;                  // IRepository<T>
using AutomotiveApp.Infrastructure.Data;               // AppDbContext
using AutomotiveApp.Application.PaymentMethods;        // DTOs
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Application.Interfaces.Repositories;                      // TransactionCategory

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IRepository<PaymentMethod> _repo;
        private readonly AppDbContext _db; // diperlukan untuk SaveChangesAsync

        public PaymentMethodsController(IRepository<PaymentMethod> repo, AppDbContext db)
        {
            _repo = repo;
            _db   = db;
        }

        /// <summary>Ambil semua payment method (data seeding harus tampil).</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodReadDto>>> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new PaymentMethodReadDto
            {
                Id         = x.Id,
                Name       = x.Name,       // enum TransactionCategory
                Status     = x.Status,
                CreatedAt = x.CreatedAt, // <-- sesuaikan dg BaseEntity kamu
                UpdatedAt = x.UpdatedAt
            });
            return Ok(dto);
        }

        /// <summary>Ambil payment method by Id.</summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PaymentMethodReadDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new PaymentMethodReadDto
            {
                Id         = x.Id,
                Name       = x.Name,
                Status     = x.Status,
                CreatedAt  = x.CreatedAt, 
                UpdatedAt  = x.UpdatedAt
            });
        }

        /// <summary>Buat payment method baru.</summary>
        [HttpPost]
        public async Task<ActionResult<PaymentMethodReadDto>> Create([FromBody] PaymentMethodCreateDto input)
        {
            var entity = new PaymentMethod
            {
                Name   = input.Name,   // enum
                Status = input.Status
            };

            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync(); // penting: BaseRepository tidak auto save

            var dto = new PaymentMethodReadDto
            {
                Id         = entity.Id,
                Name       = entity.Name,
                Status     = entity.Status,
                CreatedAt  = entity.CreatedAt, 
                UpdatedAt  = entity.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        /// <summary>Update payment method.</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PaymentMethodUpdateDto input)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            entity.Name = input.Name;
            entity.Status = input.Status;
            _repo.Update(entity);              // akan panggil MarkUpdated()
            await _db.SaveChangesAsync();      // penting: simpan perubahan

            return NoContent();
        }

        /// <summary>Delete payment method.</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _repo.Delete(entity);
            await _db.SaveChangesAsync();      // simpan perubahan
            return NoContent();
        }
    }
}
