using System.Linq.Expressions;
using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.WebAPI.Helper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Controllers.Carts
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartsController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetAll([FromQuery] Guid? userId)
        {
            var response = new ApiResponse<IEnumerable<CartReadDto>>();
            Func<IQueryable<Cart>, IQueryable<Cart>>? modifier = null;
            if (userId is Guid id)
            {
                var userExist = await _uow.UserRepo.DataExistAsync(u => u.Id == id);
                if (!userExist)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Errors = [$"User with id {id} not found"]
                    });
                }
                modifier = q => q.Where(c => c.UserId == id);
            }

            var carts = await _uow.CartRepo.GetAllAsync(modifier: modifier);
            var result = _mapper.Map<IEnumerable<CartReadDto>>(carts);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetCurrentUser()
        {
            var response = new ApiResponse<IEnumerable<CartReadDto>>();
            var userId = User.GetCurrentUserId() ?? throw new UnauthorizedAccessException();
            Expression<Func<Cart, bool>> predicate = cb => cb.UserId == userId;

            var carts = await _uow.CartRepo.FindAsync(predicate: predicate, modifier: q => q
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Session)
                .ThenInclude(s => s.Course));

            var result = _mapper.Map<IEnumerable<CartReadDto>>(carts);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartDetailsReadDto>> GetById([FromRoute] Guid id,
        [FromServices] IValidator<CartDetailsReadDto> validator)
        {
            var response = new ApiResponse<CartDetailsReadDto>();
            var cart = await _uow.CartRepo.GetByIdAsync(id,
            modifier: q => q.Include(c => c.Items));

            var cartDetails = _mapper.Map<CartDetailsReadDto>(cart);
            await validator.ValidateAndThrowAsync(cartDetails);

            response.Success = true;
            response.Data = cartDetails;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CartReadDto>> Create([FromBody] CartCreateDto request,
        [FromServices] IValidator<CartCreateDto> validator)
        {
            var response = new ApiResponse<CartReadDto>();
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CartReadDto>(validation);
            var cart = _mapper.Map<Cart>(request);

            await _uow.CartRepo.AddAsync(cart);
            await _uow.SaveChangesAsync();
            var result = _mapper.Map<CartReadDto>(cart);

            response.Success = true;
            response.Data = result;

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            var data = await _uow.CartRepo.GetByIdAsync(id, ct: ct)
            ?? throw new NotFoundException<Cart>(id);

            _uow.CartRepo.Delete(data);
            response.Data = $"Cart {id} is successfully deleted.";
            response.Success = true;
            return Ok(response);
        }

        [HttpDelete("items")]
        public async Task<ActionResult> BatchDelete([FromBody] IEnumerable<Guid> cartItemIds, CancellationToken ct)
        {
            var response = new ApiResponse<string>();

            var items = await _uow.CartItemRepo.Query()
                .Include(ci => ci.Cart)
                .Where(ci => cartItemIds.Contains(ci.Id))
                .ToListAsync(ct);

            await _uow.CartRepo.BatchDelete(items, ct);
            await _uow.SaveChangesAsync(ct);

            var cart = items.First().Cart;
            var remainingItems = await _uow.CartItemRepo.Query()
            .Where(ci => ci.CartId == cart.Id)
            .Include(ci => ci.Session)
                .ThenInclude(s => s.Course)
            .ToListAsync(ct);

            cart.TotalPrice = remainingItems.Sum(ci => ci.Session.Course.Price);
            _uow.CartRepo.Update(cart);
            await _uow.SaveChangesAsync(ct);

            response.Data = $"Items are successfully deleted from cart.";
            response.Success = true;

            return Ok(response);
        }
    }
}