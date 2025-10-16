using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutomotiveApp.Domain.Entities.Orders;
using Microsoft.AspNetCore.Authorization;
using AutomotiveApp.Shared.Dtos.Order;
using AutomotiveApp.WebAPI.Helper;
using FluentValidation;
using AutomotiveApp.Application.Interfaces;
using AutoMapper;
using AutomotiveApp.Shared.Response;
using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Shared.Exceptions;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Enums;
using AutomotiveApp.Shared.Dtos.OrderItem;
using AutomotiveApp.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using AutomotiveApp.Domain.Entities.Courses.Cart; // IRepository<>         

namespace AutomotiveApp.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }

        // GET /api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAll()
        {
            var response = new ApiResponse<IEnumerable<OrderReadDto>>();

            var items = await _uow.OrderRepo.GetAllAsync(modifier: q => q.Include(o => o.PaymentMethod));
            var dto = _mapper.Map<IEnumerable<OrderReadDto>>(items);

            response.Success = true;
            response.Data = dto;
            return Ok(response);
        }

        // GET /api/orders/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderReadDetailsDto>> GetById([FromQuery] Guid id)
        {
            var response = new ApiResponse<OrderReadDetailsDto>();
            var item = await _uow.OrderRepo.GetByIdAsync(id) ?? throw new NotFoundException<Order>(id);

            var dto = _mapper.Map<OrderReadDetailsDto>(item);
            response.Success = true;
            response.Data = dto;
            return Ok(response);
        }

        [HttpPost("instant")]
        public async Task<ActionResult<OrderReadDto>> Create([FromBody] InstantOrderCreateDto request,
        [FromServices] IValidator<InstantOrderCreateDto> validator, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            var userId = User.GetCurrentUserId() ?? throw new UnauthorizedAccessException();
            request.UserId = userId;

            await validator.ValidateAndThrowAsync(request, ct);
            await using var transaction = await _uow.BeginTransactionAsync(ct);
            try
            {
                //1. Create Order
                var order = new Order
                {
                    UserId = request.UserId,
                    PaymentMethodId = request.PaymentId,
                    Status = OrderStatus.Pending
                };

                await _uow.OrderRepo.AddAsync(order);
                await _uow.SaveChangesAsync(ct);

                //2. get the session data & create the order Item
                var session = await _uow.CourseSessionRepo.Query()
                    .Where(s => s.Id == request.SessionId)
                    .Select(s => new
                    {
                        s.Date,
                        s.Course.Price
                    })
                    .FirstOrDefaultAsync() ?? throw new NotFoundException<CourseSession>(request.SessionId);

                var orderItem = new OrderItem
                {
                    SessionId = request.SessionId,
                    Price = session.Price,
                    OrderId = order.Id,
                };

                await _uow.OrderItemRepo.AddAsync(orderItem);
                await _uow.SaveChangesAsync(ct);

                //3. set the order succesful and finalize the price (Payment method is always succesful in the demo) 
                order.Status = OrderStatus.Finished;
                order.TotalPrice = orderItem.Price;
                _uow.OrderRepo.Update(order);
                await _uow.SaveChangesAsync(ct);

                //4. Create the invoice
                var lastInvoiceNumber = await _uow.InvoiceRepo.GetLastInvoiceNumber();
                var invoice = new Invoice
                {
                    TotalPrice = order.TotalPrice,
                    InvoiceNumber = lastInvoiceNumber + 1,
                    OrderId = order.Id
                };

                await _uow.InvoiceRepo.AddAsync(invoice);
                await _uow.SaveChangesAsync(ct);

                //5. set the user Bookings
                var userBooking = new CourseBooking
                {
                    SessionId = request.SessionId,
                    UserId = request.UserId,
                };

                await _uow.CourseBookingRepo.AddAsync(userBooking);
                await _uow.SaveChangesAsync(ct);

                //6. Commit the transaction
                await transaction.CommitAsync(ct);
                response.Data = "Payment received and order completed successfully.";
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);

            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync(ct);
                }

                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> Create(
        [FromBody] OrderCreateDto request, IValidator<OrderCreateDto> validator, CancellationToken ct)
        {
            var response = new ApiResponse<string>();
            var userId = User.GetCurrentUserId() ?? throw new UnauthorizedAccessException();
            request.UserId = userId;

            await validator.ValidateAndThrowAsync(request, ct);
            await using var transaction = await _uow.BeginTransactionAsync(ct);

            try
            {
                // Get user cart 
                var cart = await _uow.CartRepo.FirstOrDefaultAsync(
                    modifier: q => q
                        .Include(c => c.Items)
                        .ThenInclude(i => i.Session)
                        .ThenInclude(s => s.Course)
                        ,
                    predicate: c => c.UserId == userId,
                    ct: ct
                ) ?? throw new NotFoundException<Cart>("User cart not found.");

                var order = new Order
                {
                    UserId = userId,
                    PaymentMethodId = request.PaymentMethodId,
                    Status = OrderStatus.Pending
                };

                await _uow.OrderRepo.AddAsync(order);
                await _uow.SaveChangesAsync(ct);

                //Get the ordered items
                var orderedItems = cart.Items.Where(i => request.CartItemIds.Contains(i.Id));
                long totalPrice = 0;

                foreach (var item in orderedItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        SessionId = item.SessionId,
                        Price = item.Session.Course.Price
                    };

                    totalPrice += orderItem.Price;
                    await _uow.OrderItemRepo.AddAsync(orderItem);
                }
                await _uow.SaveChangesAsync(ct);

                // Complete order
                order.Status = OrderStatus.Finished;
                order.TotalPrice = totalPrice;
                _uow.OrderRepo.Update(order);
                await _uow.SaveChangesAsync(ct);

                // Create invoice
                var lastInvoiceNumber = await _uow.InvoiceRepo.GetLastInvoiceNumber();
                var invoice = new Invoice
                {
                    TotalPrice = order.TotalPrice,
                    InvoiceNumber = lastInvoiceNumber + 1,
                    OrderId = order.Id
                };
                await _uow.InvoiceRepo.AddAsync(invoice);
                await _uow.SaveChangesAsync(ct);

                // Create bookings && remove the cart
                foreach (var item in orderedItems)
                {
                    var booking = new CourseBooking
                    {
                        SessionId = item.SessionId,
                        UserId = userId
                    };
                    await _uow.CourseBookingRepo.AddAsync(booking);
                }
                await _uow.SaveChangesAsync(ct);
                // Clear cart
                _uow.CartRepo.BatchDelete(orderedItems, ct);
                await _uow.SaveChangesAsync(ct);

                cart.TotalPrice = await _uow.CartItemRepo.RecalculateCartTotalAsync(request.CartId);
                _uow.CartRepo.Update(cart);
                await _uow.SaveChangesAsync(ct);

                // Commit transaction
                await transaction.CommitAsync(ct);

                response.Data = "Payment received and order completed successfully.";
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    await transaction.RollbackAsync(ct);

                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors = [ex.Message];
                return BadRequest(response);
            }
        }


    }

    // // PUT /api/orders/{id}
    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> Update(Guid id, [FromBody] OrderUpdateDto input)
    // {
    //     var entity = await _repo.GetByIdAsync(id);
    //     if (entity == null) return NotFound();

    //     var pmExists = await _db.PaymentMethods.AnyAsync(p => p.Id == input.PaymentMethodId);
    //     if (!pmExists) return BadRequest("PaymentMethodId tidak valid.");

    //     entity.PaymentMethodId = input.PaymentMethodId;
    //     entity.Status = input.Status;

    //     // UpdatedAt via BaseRepository.Update -> MarkUpdated()
    //     _repo.Update(entity);
    //     await _db.SaveChangesAsync();

    //     return NoContent();
    // }

    // // DELETE /api/orders/{id}
    // [HttpDelete("{id:guid}")]
    // public async Task<IActionResult> Delete(Guid id)
    // {
    //     var entity = await _repo.GetByIdAsync(id);
    //     if (entity == null) return NotFound();

    //     _repo.Delete(entity);
    //     await _db.SaveChangesAsync();

    //     return NoContent();
    // }
}

