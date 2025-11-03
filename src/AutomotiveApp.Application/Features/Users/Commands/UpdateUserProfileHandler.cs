using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class UpdateUserProfileHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<UpdateUserProfile, bool>
    {
        public async Task<bool> Handle(UpdateUserProfile req, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == req.UserProfileUpdateDto.CurrentUserId, cancellationToken: cancellationToken);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {req.UserProfileUpdateDto.CurrentUserId} not found");

            var dto = req.UserProfileUpdateDto;

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                existingUser.UserName = dto.UserName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email))
                existingUser.Email = dto.Email.Trim();

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                existingUser.PhoneNumber = dto.PhoneNumber.Trim();

            mapper.Map(req.UserProfileUpdateDto, existingUser);
            
            var result = await userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            return true;
        }
    }
}
