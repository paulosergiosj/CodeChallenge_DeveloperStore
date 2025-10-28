using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for AuthenticateUserHandler.
/// </summary>
public class AuthenticateUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticateUserHandler _handler;

    public AuthenticateUserHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

        _unitOfWork.Users.Returns(_userRepository);

        _handler = new AuthenticateUserHandler(_unitOfWork, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact(DisplayName = "Given valid credentials When authenticating user Then returns success response")]
    public async Task Handle_ValidCredentials_ReturnsSuccessResponse()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, command.Password);
        var token = AuthenticateUserHandlerTestData.GenerateValidToken();
        var expectedResult = AuthenticateUserHandlerTestData.GenerateValidResult(user, token);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);
        _jwtTokenGenerator.GenerateToken(user)
            .Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(token);
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Username);
        result.Role.Should().Be(user.Role.ToString());
        result.Id.Should().Be(user.Id);
        result.Phone.Should().Be(user.Phone);
    }

    [Fact(DisplayName = "Given non-existent user When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_NonExistentUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Invalid credentials");
    }

    [Fact(DisplayName = "Given invalid password When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, "DifferentPassword123!");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Invalid credentials");
    }

    [Fact(DisplayName = "Given inactive user When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_InactiveUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateInactiveUser(command.Email, command.Password);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("User is not active");
    }

    [Fact(DisplayName = "Given suspended user When authenticating Then throws UnauthorizedAccessException")]
    public async Task Handle_SuspendedUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateSuspendedUser(command.Email, command.Password);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("User is not active");
    }

    [Fact(DisplayName = "Given valid credentials When authenticating Then calls GetByEmailAsync")]
    public async Task Handle_ValidCredentials_CallsGetByEmailAsync()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, command.Password);
        var token = AuthenticateUserHandlerTestData.GenerateValidToken();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);
        _jwtTokenGenerator.GenerateToken(user)
            .Returns(token);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid credentials When authenticating Then calls VerifyPassword")]
    public async Task Handle_ValidCredentials_CallsVerifyPassword()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, command.Password);
        var token = AuthenticateUserHandlerTestData.GenerateValidToken();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);
        _jwtTokenGenerator.GenerateToken(user)
            .Returns(token);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordHasher.Received(1).VerifyPassword(command.Password, user.Password);
    }

    [Fact(DisplayName = "Given valid credentials When authenticating Then calls GenerateToken")]
    public async Task Handle_ValidCredentials_CallsGenerateToken()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, command.Password);
        var token = AuthenticateUserHandlerTestData.GenerateValidToken();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);
        _jwtTokenGenerator.GenerateToken(user)
            .Returns(token);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _jwtTokenGenerator.Received(1).GenerateToken(user);
    }

    [Fact(DisplayName = "Given valid credentials When authenticating Then returns correct result structure")]
    public async Task Handle_ValidCredentials_ReturnsCorrectResultStructure()
    {
        // Arrange
        var command = AuthenticateUserHandlerTestData.GenerateValidCommand();
        var user = AuthenticateUserHandlerTestData.GenerateValidUser(command.Email, command.Password);
        var token = AuthenticateUserHandlerTestData.GenerateValidToken();

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password)
            .Returns(true);
        _jwtTokenGenerator.GenerateToken(user)
            .Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AuthenticateUserResult>();
        result.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().NotBeNullOrEmpty();
        result.Name.Should().NotBeNullOrEmpty();
        result.Role.Should().NotBeNullOrEmpty();
        result.Id.Should().Be(user.Id); // Should match the user's ID
        result.Phone.Should().NotBeNullOrEmpty();
    }
}
