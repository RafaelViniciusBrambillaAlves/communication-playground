using System.Data.Common;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.ServiceModel;
using Microsoft.EntityFrameworkCore;
using SoapService.Contracts;
using SoapService.Contracts.Faults;
using SoapService.Contracts.Messages;
using SoapService.Data;

namespace SoapService.Services
{
    public class UserSoapService(AppDbContext db, ILogger<UserSoapService> logger): IUserSoapService
    {
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                ValidateName(request.Name);
                ValidateEmail(request.Email);
                ValidateAge(request.Age);

                var user = new User
                {
                    Name = request.Name.Trim(),
                    Email = request.Email.Trim().ToLower(),
                    Age = request.Age
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return new CreateUserResponse { User = ToUserData(user) };
            }
            catch (ArgumentException ex)
            {
                throw ValidationFaultFor(ex);
            }
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user is null)
                throw NotFoundFaultFor(request.Id);

            return new GetUserResponse { User = ToUserData(user) };
        }

        public async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request)
        {
            var users = await db.Users.AsNoTracking().ToListAsync();

            return new GetAllUsersResponse
            {
                Users = users.Select(ToUserData).ToList() 
            };
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

                if (user is null)
                    throw NotFoundFaultFor(request.Id);

                if (request.Name is not null)
                {
                    ValidateName(request.Name);
                    user.Name = request.Name.Trim();
                }
                if (request.Email is not null)
                {
                    ValidateEmail(request.Email);
                    user.Email = request.Email.Trim().ToLower();
                }
                if (request.Age is not null)
                {
                    ValidateAge(request.Age.Value);
                    user.Age = request.Age.Value;
                }

                user.UpdatedAt = DateTime.UtcNow;

                await db.SaveChangesAsync();

                return new UpdateUserResponse { User = ToUserData(user) };
            }
            catch (FaultException)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw ValidationFaultFor(ex);    
            }
        }

        public async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user is null)
                throw NotFoundFaultFor(request.Id);

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return new DeleteUserResponse { Success = true };
        }


        // Helpers

        private static UserData ToUserData(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Age = user.Age,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        private FaultException<UserNotFoundFault> NotFoundFaultFor(Guid id)
        {
            var message = $"User with id '{id}' was not found";
            logger.LogWarning("User {Id} not found", id);
            return new FaultException<UserNotFoundFault>(
                new UserNotFoundFault { Message = message, UserId = id },
                new FaultReason(message));
        } 

        private FaultException<ValidationFault> ValidationFaultFor(ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            return new FaultException<ValidationFault>(
                new ValidationFault { Message = ex.Message },
                new FaultReason(ex.Message));
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 2 || name.Trim().Length > 100)
                throw new ArgumentException("Name must be between 2 and 100 characters", nameof(name));
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));
        }

        private static void ValidateAge(int age)
        {
            if (age < 18)
                throw new ArgumentException("User must be at least 18 years old", nameof(age));
        }

    }
}