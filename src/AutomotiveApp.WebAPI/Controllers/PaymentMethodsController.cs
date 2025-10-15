using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomotiveApp.Domain.Entities.Payments;          
using AutomotiveApp.Infrastructure.Data;             
using AutomotiveApp.Application.PaymentMethods;      
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.WebAPI.Dto.PaymentMethods;
using Microsoft.AspNetCore.Http;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase
    {
        private static readonly HashSet<string> AllowedExt =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".svg" };
        
        private string? BuildImageUrl(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return $"{baseUrl}/images/PaymentMethod/{fileName}";
        }

        private readonly IRepository<PaymentMethod> _repo;
        private readonly AppDbContext _db;

        public PaymentMethodsController(IRepository<PaymentMethod> repo, AppDbContext db, IFileStorage imageStorage)
        {
            _repo = repo;
            _db = db;
            _imageStorage = imageStorage;
        }
        private readonly IFileStorage _imageStorage;


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodReadDto>>> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(x => new PaymentMethodReadDto
            {
                Id = x.Id,
                Name = x.Name,      
                Status = x.Status,
                CreatedAt = x.CreatedAt, 
                UpdatedAt = x.UpdatedAt,
                ImageUrl = BuildImageUrl(x.ImageFileName)
            });
            return Ok(dto);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PaymentMethodReadDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new PaymentMethodReadDto
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                ImageUrl = BuildImageUrl(x.ImageFileName)
            });
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] PaymentMethodCreateForm form)
        {
            var response = new ApiResponse<PaymentMethodReadDto>();

            if (form is null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = new[] { "Form data is required" };
                return BadRequest(response);
            }

            var newId = Guid.NewGuid();
            string? imageFileName = null;

            if (form.FileImageName is not null && form.FileImageName.Length > 0)
            {
                var originalName = form.FileImageName.FileName ?? string.Empty;
                var ext = Path.GetExtension(originalName).ToLowerInvariant();

                if (!AllowedExt.Contains(ext))
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new[] { "Invalid image file type. Only JPG, JPEG, PNG, or SVG formats are supported." };
                    return BadRequest(response);
                }

                imageFileName = $"{newId}{ext}";

                try
                {
                    await using var stream = form.FileImageName.OpenReadStream();
                    await _imageStorage.SaveFileAsync<PaymentMethod>(stream, imageFileName);
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new[] { $"Failed to save image: {ex.Message}" };
                    return BadRequest(response);
                }
            }

            var entity = new PaymentMethod
            {
                Id = newId,
                Name = form.Name ?? string.Empty,   
                Status = form.Status,
                ImageFileName = imageFileName
            };

            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync();

            var dto = new PaymentMethodReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ImageUrl = BuildImageUrl(entity.ImageFileName)
            };

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = dto;
            return Ok(response);
        }


        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] PaymentMethodUpdateRequest request)
        {
            var response = new ApiResponse<PaymentMethodReadDto>();
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Errors = new[] { $"PaymentMethod {id} not found." };
                return NotFound(response);
            }

            entity.Name = request.Name;
            entity.Status = request.Status;

            if (request.FileImageName is not null && request.FileImageName.Length > 0)
            {
                var ext = Path.GetExtension(request.FileImageName.FileName ?? string.Empty).ToLowerInvariant();
                if (!AllowedExt.Contains(ext))
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors = new[] { "Invalid image file type. Only JPG, JPEG, PNG, or SVG formats are supported." };
                    return BadRequest(response);
                }

                var newFileName = $"{id}{ext}";

                // kalau sebelumnya ada file & berbeda ekstensi/filename, hapus dulu
                if (!string.IsNullOrWhiteSpace(entity.ImageFileName) &&
                    !string.Equals(entity.ImageFileName, newFileName, StringComparison.OrdinalIgnoreCase))
                {
                    await _imageStorage.DeleteFileAsync<PaymentMethod>(entity.ImageFileName);
                }

                // replace atau save yang baru 
                await _imageStorage.ReplaceFileAsync<PaymentMethod>(newFileName, request.FileImageName.OpenReadStream());
                entity.ImageFileName = newFileName;
            }

            _repo.Update(entity);             
            await _db.SaveChangesAsync();      
            var dto = new PaymentMethodReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ImageUrl = BuildImageUrl(entity.ImageFileName)
            };
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = dto;
            return Ok(response);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _repo.Delete(entity);
            await _db.SaveChangesAsync();     
            return NoContent();
        }

        [HttpPost("json")]
        [Consumes("application/json")]
        public async Task<ActionResult> CreateJson([FromBody] PaymentMethodCreateDto body)
        {
            var response = new ApiResponse<PaymentMethodReadDto>();
            var newId = Guid.NewGuid();

            var entity = new PaymentMethod
            {
                Id = newId,
                Name = body.Name,
                Status = body.Status,
                ImageFileName = body.ImageFilename 
            };

            await _repo.AddAsync(entity);
            await _db.SaveChangesAsync();

            var dto = new PaymentMethodReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ImageUrl = BuildImageUrl(entity.ImageFileName)
            };

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = dto;
            return Ok(response);
        }

        [HttpPut("{id:guid}/json")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateJson(Guid id, [FromBody] PaymentMethodUpdateDto body)
        {
            var response = new ApiResponse<PaymentMethodReadDto>();
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Errors = new[] { $"PaymentMethod {id} not found." };
                return NotFound(response);
            }

            entity.Name = body.Name;
            entity.Status = body.Status;
            if (!string.IsNullOrWhiteSpace(body.ImageFilename))
                entity.ImageFileName = body.ImageFilename;

            _repo.Update(entity);
            await _db.SaveChangesAsync();

            var dto = new PaymentMethodReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ImageUrl = BuildImageUrl(entity.ImageFileName)
            };

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = dto;
            return Ok(response);
        }

    }
}
