using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for GetUserHandler tests.
/// </summary>
public static class GetUserHandlerTestData
{
    /// <summary>
    /// Generates a valid GetUserCommand with randomized data.
    /// </summary>
    /// <returns>A valid GetUserCommand with randomly generated data.</returns>
    public static GetUserCommand GenerateValidCommand()
    {
        var faker = new Faker();
        
        return new GetUserCommand(faker.Random.Int(1, 1000));
    }

    /// <summary>
    /// Generates a valid GetUserCommand with specific user number.
    /// </summary>
    /// <param name="userNumber">The user number to use</param>
    /// <returns>A valid GetUserCommand with the specified user number.</returns>
    public static GetUserCommand GenerateValidCommand(int userNumber)
    {
        return new GetUserCommand(userNumber);
    }

    /// <summary>
    /// Generates an invalid GetUserCommand for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid GetUserCommand for testing validation errors.</returns>
    public static GetUserCommand GenerateInvalidCommand()
    {
        return new GetUserCommand(0); // Invalid user number
    }

    /// <summary>
    /// Generates a valid User entity for testing.
    /// </summary>
    /// <param name="userNumber">The user number to use</param>
    /// <param name="role">The role to use</param>
    /// <param name="status">The status to use</param>
    /// <returns>A valid User entity.</returns>
    public static User GenerateValidUser(int? userNumber = null, UserRole? role = null, UserStatus? status = null)
    {
        var faker = new Faker();
        
        var user = User.Create(
            username: faker.Internet.UserName(),
            email: faker.Internet.Email(),
            phone: "+5511999999999", // Valid phone format
            hashedPassword: "HashedPass123!",
            role: role ?? UserRole.Customer
        );

        // Set user number using reflection if provided
        if (userNumber.HasValue)
        {
            typeof(User).GetProperty("UserNumber")?.SetValue(user, userNumber.Value);
        }

        // Set status using reflection if different from default
        if (status.HasValue)
        {
            typeof(User).GetProperty("Status")?.SetValue(user, status.Value);
        }

        return user;
    }

    /// <summary>
    /// Generates a valid GetUserResult for testing.
    /// </summary>
    /// <param name="user">The user to generate result from</param>
    /// <returns>A valid GetUserResult.</returns>
    public static GetUserResult GenerateValidResult(User user)
    {
        return new GetUserResult
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            Status = user.Status
        };
    }

    /// <summary>
    /// Generates a valid GetUserResult with specific data for testing.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="name">The name to use</param>
    /// <param name="email">The email to use</param>
    /// <param name="phone">The phone to use</param>
    /// <param name="role">The role to use</param>
    /// <param name="status">The status to use</param>
    /// <returns>A valid GetUserResult.</returns>
    public static GetUserResult GenerateValidResult(
        Guid? id = null, 
        string? name = null, 
        string? email = null, 
        string? phone = null, 
        UserRole? role = null, 
        UserStatus? status = null)
    {
        var faker = new Faker();
        
        return new GetUserResult
        {
            Id = id ?? faker.Random.Guid(),
            Name = name ?? faker.Internet.UserName(),
            Email = email ?? faker.Internet.Email(),
            Phone = phone ?? "+5511999999999", // Valid phone format
            Role = role ?? UserRole.Customer,
            Status = status ?? UserStatus.Active
        };
    }
}
