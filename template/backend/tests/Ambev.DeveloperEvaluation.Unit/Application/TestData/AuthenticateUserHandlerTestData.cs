using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for AuthenticateUserHandler tests.
/// </summary>
public static class AuthenticateUserHandlerTestData
{
    /// <summary>
    /// Generates a valid AuthenticateUserCommand with randomized data.
    /// </summary>
    /// <returns>A valid AuthenticateUserCommand with randomly generated data.</returns>
    public static AuthenticateUserCommand GenerateValidCommand()
    {
        var faker = new Faker();
        
        return new AuthenticateUserCommand
        {
            Email = faker.Internet.Email(),
            Password = GenerateValidPassword()
        };
    }

    /// <summary>
    /// Generates a valid AuthenticateUserCommand with specific email.
    /// </summary>
    /// <param name="email">The email to use</param>
    /// <returns>A valid AuthenticateUserCommand with the specified email.</returns>
    public static AuthenticateUserCommand GenerateValidCommand(string email)
    {
        return new AuthenticateUserCommand
        {
            Email = email,
            Password = GenerateValidPassword()
        };
    }

    /// <summary>
    /// Generates an invalid AuthenticateUserCommand for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid AuthenticateUserCommand for testing validation errors.</returns>
    public static AuthenticateUserCommand GenerateInvalidCommand()
    {
        return new AuthenticateUserCommand
        {
            Email = string.Empty, // Invalid email
            Password = string.Empty // Invalid password
        };
    }

    /// <summary>
    /// Generates a valid password that meets complexity requirements.
    /// </summary>
    /// <returns>A valid password string.</returns>
    public static string GenerateValidPassword()
    {
        return "ValidPass123!";
    }

    /// <summary>
    /// Generates an invalid password for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid password string.</returns>
    public static string GenerateInvalidPassword()
    {
        return "weak";
    }

    /// <summary>
    /// Generates a valid User entity for testing.
    /// </summary>
    /// <param name="email">The email to use</param>
    /// <param name="password">The password to use</param>
    /// <param name="role">The role to use</param>
    /// <param name="status">The status to use</param>
    /// <returns>A valid User entity.</returns>
    public static User GenerateValidUser(
        string? email = null, 
        string? password = null, 
        UserRole? role = null, 
        UserStatus? status = null)
    {
        var faker = new Faker();
        
        var user = User.Create(
            username: faker.Internet.UserName(),
            email: email ?? faker.Internet.Email(),
            phone: "+5511999999999", // Valid phone format
            hashedPassword: password ?? GenerateValidPassword(),
            role: role ?? UserRole.Customer
        );

        // Set status using reflection if different from default
        if (status.HasValue)
        {
            typeof(User).GetProperty("Status")?.SetValue(user, status.Value);
        }

        return user;
    }

    /// <summary>
    /// Generates an inactive User entity for testing negative scenarios.
    /// </summary>
    /// <param name="email">The email to use</param>
    /// <param name="password">The password to use</param>
    /// <returns>An inactive User entity.</returns>
    public static User GenerateInactiveUser(string? email = null, string? password = null)
    {
        var user = GenerateValidUser(email, password, UserRole.Customer, UserStatus.Inactive);
        return user;
    }

    /// <summary>
    /// Generates a suspended User entity for testing negative scenarios.
    /// </summary>
    /// <param name="email">The email to use</param>
    /// <param name="password">The password to use</param>
    /// <returns>A suspended User entity.</returns>
    public static User GenerateSuspendedUser(string? email = null, string? password = null)
    {
        var user = GenerateValidUser(email, password, UserRole.Customer, UserStatus.Suspended);
        return user;
    }

    /// <summary>
    /// Generates a valid AuthenticateUserResult for testing.
    /// </summary>
    /// <param name="user">The user to generate result from</param>
    /// <param name="token">The token to use</param>
    /// <returns>A valid AuthenticateUserResult.</returns>
    public static AuthenticateUserResult GenerateValidResult(User user, string? token = null)
    {
        var faker = new Faker();
        
        return new AuthenticateUserResult
        {
            Token = token ?? faker.Random.String(50),
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString()
        };
    }

    /// <summary>
    /// Generates a valid JWT token string for testing.
    /// </summary>
    /// <returns>A valid JWT token string.</returns>
    public static string GenerateValidToken()
    {
        var faker = new Faker();
        return faker.Random.String(100);
    }
}
