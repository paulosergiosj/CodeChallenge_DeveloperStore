using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for ActiveUserSpecification tests
/// to ensure consistency across test cases.
/// </summary>
public static class ActiveUserSpecificationTestData
{
    /// <summary>
    /// Generates a valid User entity with the specified status.
    /// Since User is a Rich Domain Model, we use the User.Create() factory method
    /// and then modify the status using the appropriate domain methods.
    /// </summary>
    /// <param name="status">The UserStatus to set for the generated user.</param>
    /// <returns>A valid User entity with randomly generated data and specified status.</returns>
    public static User GenerateUser(UserStatus status)
    {
        var faker = new Faker();
        
        var user = User.Create(
            username: faker.Internet.UserName(),
            email: faker.Internet.Email(),
            phone: $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}",
            hashedPassword: $"Test@{faker.Random.Number(100, 999)}",
            role: faker.PickRandom(UserRole.Customer, UserRole.Admin)
        );

        // Set the desired status using domain methods
        switch (status)
        {
            case UserStatus.Active:
                user.Activate();
                break;
            case UserStatus.Suspended:
                user.Suspend();
                break;
            case UserStatus.Inactive:
                user.Deactivate();
                break;
        }

        return user;
    }
}
