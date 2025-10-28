using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class UserTests
{
    /// <summary>
    /// Tests that when a suspended user is activated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Suspend();

        // Act
        user.Activate();

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is suspended, their status changes to Suspended.
    /// </summary>
    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        user.Suspend();

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is deactivated, their status changes to Inactive.
    /// </summary>
    [Fact(DisplayName = "User status should change to Inactive when deactivated")]
    public void Given_ActiveUser_When_Deactivated_Then_StatusShouldBeInactive()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        user.Deactivate();

        // Assert
        Assert.Equal(UserStatus.Inactive, user.Status);
    }

    /// <summary>
    /// Tests that validation passes when all user properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid user data")]
    public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        var result = user.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when user properties are invalid.
    /// Since User is a Rich Domain Model, we test validation through the UserValidator.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid user data")]
    public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange - Test validation rules directly through UserValidator
        var validator = new Ambev.DeveloperEvaluation.Domain.Validation.UserValidator();
        
        // Create a user with invalid data by using reflection to bypass the factory method
        var invalidUser = new User();
        
        // Use reflection to set invalid data for testing validation rules
        var usernameProperty = typeof(User).GetProperty("Username");
        var emailProperty = typeof(User).GetProperty("Email");
        var phoneProperty = typeof(User).GetProperty("Phone");
        var passwordProperty = typeof(User).GetProperty("Password");
        var roleProperty = typeof(User).GetProperty("Role");
        
        usernameProperty?.SetValue(invalidUser, ""); // Invalid username
        emailProperty?.SetValue(invalidUser, "invalid-email"); // Invalid email
        phoneProperty?.SetValue(invalidUser, "invalid-phone"); // Invalid phone
        passwordProperty?.SetValue(invalidUser, "weak"); // Invalid password
        roleProperty?.SetValue(invalidUser, UserRole.None); // Invalid role

        // Act
        var result = validator.Validate(invalidUser);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    /// <summary>
    /// Tests that user creation with invalid data throws validation exception.
    /// </summary>
    [Fact(DisplayName = "User creation with invalid data should throw validation exception")]
    public void Given_InvalidUserData_When_CreatingUser_Then_ShouldThrowValidationException()
    {
        // Arrange & Act & Assert
        Assert.Throws<FluentValidation.ValidationException>(() =>
        {
            User.Create(
                username: "", // invalid
                email: UserTestData.GenerateInvalidEmail(), // invalid
                phone: UserTestData.GenerateInvalidPhone(), // invalid
                hashedPassword: UserTestData.GenerateInvalidPassword(),
                role: UserRole.None // invalid
            );
        });
    }

    /// <summary>
    /// Tests that user properties can be updated correctly.
    /// </summary>
    [Fact(DisplayName = "User properties should be updated correctly")]
    public void Given_ValidUser_When_UpdatingProperties_Then_PropertiesShouldBeUpdated()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        var newUsername = UserTestData.GenerateValidUsername();
        var newEmail = UserTestData.GenerateValidEmail();
        var newPhone = UserTestData.GenerateValidPhone();
        var newPassword = UserTestData.GenerateValidPassword();

        // Act
        user.SetUserName(newUsername);
        user.SetEmail(newEmail);
        user.SetPhone(newPhone);
        user.SetPassword(newPassword);

        // Assert
        Assert.Equal(newUsername, user.Username);
        Assert.Equal(newEmail, user.Email);
        Assert.Equal(newPhone, user.Phone);
        Assert.Equal(newPassword, user.Password);
        Assert.NotNull(user.UpdatedAt);
    }
}
