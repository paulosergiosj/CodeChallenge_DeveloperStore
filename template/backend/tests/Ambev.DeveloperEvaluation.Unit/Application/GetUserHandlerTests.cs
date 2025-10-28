using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for GetUserHandler.
/// </summary>
public class GetUserHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();

        _unitOfWork.Users.Returns(_userRepository);

        _handler = new GetUserHandler(_unitOfWork, _mapper);
    }

    [Fact(DisplayName = "Given valid command When getting user Then returns success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Username);
        result.Email.Should().Be(user.Email);
        result.Phone.Should().Be(user.Phone);
        result.Role.Should().Be(user.Role);
        result.Status.Should().Be(user.Status);
    }

    [Fact(DisplayName = "Given invalid command When getting user Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateInvalidCommand();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Errors.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Given non-existent user When getting user Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"User with number {command.UserNumber} not found");
    }

    [Fact(DisplayName = "Given valid command When getting user Then calls GetByUserNumberAsync")]
    public async Task Handle_ValidCommand_CallsGetByUserNumberAsync()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When getting user Then calls Map")]
    public async Task Handle_ValidCommand_CallsMap()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<GetUserResult>(user);
    }

    [Fact(DisplayName = "Given valid command When getting user Then returns correct result structure")]
    public async Task Handle_ValidCommand_ReturnsCorrectResultStructure()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<GetUserResult>();
        result.Id.Should().Be(user.Id); // Should match the user's ID
        result.Name.Should().NotBeNullOrEmpty();
        result.Email.Should().NotBeNullOrEmpty();
        result.Phone.Should().NotBeNullOrEmpty();
        result.Role.Should().BeOneOf(UserRole.Customer, UserRole.Admin);
        result.Status.Should().BeOneOf(UserStatus.Active, UserStatus.Inactive, UserStatus.Suspended);
    }

    [Fact(DisplayName = "Given user with different roles When getting user Then returns correct role")]
    public async Task Handle_UserWithDifferentRoles_ReturnsCorrectRole()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber, UserRole.Admin);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Role.Should().Be(UserRole.Admin);
    }

    [Fact(DisplayName = "Given user with different status When getting user Then returns correct status")]
    public async Task Handle_UserWithDifferentStatus_ReturnsCorrectStatus()
    {
        // Arrange
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = GetUserHandlerTestData.GenerateValidUser(command.UserNumber, UserRole.Customer, UserStatus.Inactive);
        var expectedResult = GetUserHandlerTestData.GenerateValidResult(user);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<GetUserResult>(user)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Status.Should().Be(UserStatus.Inactive);
    }
}
