using AutomotiveApp.Shared.Dtos.CartItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Application.Interfaces;
using AutoMapper;
using AutomotiveApp.Shared.Response;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage;
using AutomotiveApp.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using AutomotiveApp.WebAPI.Helper;

namespace AutomotiveApp.WebAPI.Controllers.Carts
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            var items = await _uow.CartItemRepo.GetAllAsync(modifier:
                q => q.Include(ci => ci.Session)
                    .ThenInclude(s => s.Course)
                    .ThenInclude(c => c.Category)
            );

            var dto = _mapper.Map<IEnumerable<CartItemReadDto>>(items);
            response.Success = true;
            response.Data = dto;
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartItemReadDto>> GetById([FromRoute] Guid id)
        {
            var response = new ApiResponse<CartItemReadDto>();
            var item = await _uow.CartItemRepo.GetByIdAsync(id,
            modifier:
                q => q.Include(ci => ci.Session)
                    .ThenInclude(s => s.Course)
                    .ThenInclude(c => c.Category));

            var dto = _mapper.Map<CartItemReadDto>(item);

            response.Success = true;
            response.Data = dto;
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemReadDto>> Create([FromBody] CartItemCreateDto request,
        [FromServices] IValidator<CartItemCreateDto> validator
        )
        {
            var response = new ApiResponse<CartItemReadDto>();
            await validator.ValidateAndThrowAsync(request);
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
                    response.Errors = [ex.Message];
                    return BadRequest(response);
                }
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CartItemReadDto>> Update([FromRoute] Guid id,
        [FromBody] CartItemUpdateDto request, [FromServices] IValidator<CartItemUpdateDto> validator)
        {
            request.Id = id;
            await validator.ValidateAndThrowAsync(request);

            var response = new ApiResponse<CartItemReadDto>();
            var data = await _uow.CartItemRepo.GetByIdAsync(id)
            ?? throw new NotFoundException<CartItem>(id);

            var newData = _mapper.Map(request, data);
            _uow.CartItemRepo.Update(newData);
            await _uow.SaveChangesAsync();

            var result = await _uow.CartItemRepo.GetByIdAsync(id, modifier: q => q
                        .Include(ci => ci.Session)
                            .ThenInclude(s => s.Course)
                                .ThenInclude(c => c.Category));

            var dto = _mapper.Map<CartItemReadDto>(result);

            response.Success = true;
            response.Data = dto;
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            var data = await _uow.CartItemRepo.GetByIdAsync(id, ct: ct)
            ?? throw new NotFoundException<CartItem>(id);

            _uow.CartItemRepo.Delete(data);
            await _uow.SaveChangesAsync();
            response.Data = $"Cart item {id} is successfully deleted.";
            response.Success = true;
            return Ok(response);
        }
    }
}