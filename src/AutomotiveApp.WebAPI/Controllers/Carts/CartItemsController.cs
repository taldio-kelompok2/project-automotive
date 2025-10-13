using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Shared.Dtos.CartItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Application.Interfaces;
using AutoMapper;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Shared.Enums;
using System.Linq.Expressions;
using FluentValidation;
using AutomotiveApp.Domain.Entities.Courses;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using AutomotiveApp.Shared.Exceptions;

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartItemsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetAll()
        {
            var response = new ApiResponse<IEnumerable<CartItemReadDto>>();
            try
            {
                var items = await _uow.CartItemRepo.GetAllAsync(modifier:
                    q => q.Include(ci => ci.Session)
                        .ThenInclude(s => s.Course)
                        .ThenInclude(c => c.Category)
                );

                var dto = _mapper.Map<IEnumerable<CartItemReadDto>>(items);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = dto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartItemReadDto>> GetById([FromRoute] Guid id)
        {
            var response = new ApiResponse<CartItemReadDto>();
            try
            {
                var item = await _uow.CartItemRepo.GetByIdAsync(id,
                modifier:
                    q => q.Include(ci => ci.Session)
                        .ThenInclude(s => s.Course)
                        .ThenInclude(c => c.Category));

                var dto = _mapper.Map<CartItemReadDto>(item);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = dto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemReadDto>> Create([FromBody] CartItemCreateDto request,
        [FromServices] IValidator<CartItemCreateDto> validator
        )
        {
            var response = new ApiResponse<CartItemReadDto>();
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CartItemReadDto>(validation);
            var cartItem = _mapper.Map<CartItem>(request);

            using (var transaction = await _uow.BeginTransactionAsync())
            {
                try
                {

                    await _uow.CartItemRepo.AddAsync(cartItem);
                    await _uow.SaveChangesAsync();

                    var cart = await _uow.CartRepo.GetByIdAsync(cartItem.CartId);
                    var itemPrice = await _uow.CartItemRepo.RecalculateCartTotalAsync(cartItem.CartId);
                    cart!.TotalPrice = itemPrice;

                    await _uow.SaveChangesAsync();

                    var result = await _uow.CartItemRepo.GetByIdAsync(cartItem.Id, modifier: q => q
                        .Include(ci => ci.Session)
                            .ThenInclude(s => s.Course)
                                .ThenInclude(c => c.Category));

                    var dto = _mapper.Map<CartItemReadDto>(result);

                    await transaction.CommitAsync();

                    response.Success = true;
                    response.StatusCode = HttpCode.OK;
                    response.Data = dto;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    if (transaction != null && transaction.GetDbTransaction().Connection != null)
                    {
                        await transaction.RollbackAsync();
                    }
                    response.Success = false;
                    response.StatusCode = HttpCode.BadRequest;
                    response.Errors = [ex.Message];
                    return BadRequest(response);
                }
            }
        }

        // [HttpPatch("{id:guid}")]
        // public async Task<ActionResult<CartItemReadDto>> Update(Guid id, [FromForm] CartItemUpdateDto input)
        // {
        //     if (!ModelState.IsValid) return BadRequest(ModelState);

        //     var entity = await _db.CartItems.FindAsync(id);
        //     if (entity == null) return NotFound("Cart Item Not Found");

        //     entity.SessionId = input.SessionId;
        //     entity.MarkUpdated();

        //     await _db.SaveChangesAsync();

        //     var dto = new CartItemReadDto
        //     {
        //         Id = entity.Id,
        //         CartId = entity.CartId,
        //         SessionId = entity.SessionId,
        //         CreatedAt = entity.CreatedAt,
        //         UpdatedAt = entity.UpdatedAt
        //     };

        //     return Ok(dto);
        // }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            try
            {
                var data = await _uow.CartItemRepo.GetByIdAsync(id, ct: ct)
                ?? throw new NotFoundException<CartItem>(id);

                _uow.CartItemRepo.Delete(data);
                response.Data = $"Cart item {id} is successfully deleted.";
                response.Success = true;
                response.StatusCode = HttpCode.OK;
                return Ok(response);
            }
            catch (NotFoundException<CartItem> ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.NotFound;
                response.Errors = [ex.Message];
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }
    }
}