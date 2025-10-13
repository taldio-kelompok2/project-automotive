using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Shared.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Controllers.User
{
    [ApiController]
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
            try
            {
                Func<IQueryable<Cart>, IQueryable<Cart>>? modifier = null;
                if (userId is Guid id)
                {
                    var userExist = await _uow.UserRepo.DataExistAsync(u => u.Id == id);
                    if (!userExist)
                    {
                        return NotFound(new ApiResponse<string>
                        {
                            Success = false,
                            StatusCode = HttpCode.NotFound,
                            Errors = [$"User with id {id} not found"]
                        });
                    }
                    modifier = q => q.Where(c => c.UserId == id);
                }

                var carts = await _uow.CartRepo.GetAllAsync(modifier: modifier);
                var result = _mapper.Map<IEnumerable<CartReadDto>>(carts);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

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
        public async Task<ActionResult<CartDetailsReadDto>> GetById([FromRoute] Guid id,
        [FromServices] IValidator<CartDetailsReadDto> validator)
        {
            var response = new ApiResponse<CartDetailsReadDto>();
            try
            {
                var cart = await _uow.CartRepo.GetByIdAsync(id,
                modifier: q => q.Include(c => c.Items));

                var cartDetails = _mapper.Map<CartDetailsReadDto>(cart);
                var validation = await validator.ValidateAsync(cartDetails);
                if (!validation.IsValid) return HandleValidationFailure<CartDetailsReadDto>(validation);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = cartDetails;

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
        public async Task<ActionResult<CartReadDto>> Create([FromBody] CartCreateDto request,
        [FromServices] IValidator<CartCreateDto> validator, CancellationToken ct)
        {
            var response = new ApiResponse<CartReadDto>();
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid) return HandleValidationFailure<CartReadDto>(validation);
            var cart = _mapper.Map<Cart>(request);

            try
            {
                await _uow.CartRepo.AddAsync(cart);
                await _uow.SaveChangesAsync();
                var result = _mapper.Map<CartReadDto>(cart);

                response.Success = true;
                response.StatusCode = HttpCode.OK;
                response.Data = result;

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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            try
            {
                var data = await _uow.CartRepo.GetByIdAsync(id, ct: ct)
                ?? throw new NotFoundException<Cart>(id);

                _uow.CartRepo.Delete(data);
                response.Data = $"Cart {id} is successfully deleted.";
                response.Success = true;
                response.StatusCode = HttpCode.OK;
                return Ok(response);
            }
            catch (NotFoundException<Cart> ex)
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