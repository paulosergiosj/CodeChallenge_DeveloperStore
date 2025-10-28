using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateUserHandler"/> class.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Users.Returns(_userRepository);
        _handler = new CreateUserHandler(_unitOfWork, _mapper, _passwordHasher);
    }

    /// <summary>
    /// Tests that a valid user creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var hashedPassword = "HashedPass123!";
        var user = User.Create(
            command.Username,
            command.Email,
            command.Phone,
            hashedPassword,
            command.Role
        );

        var result = new CreateUserResult
        {
            Id = user.Id
        };

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);
        _passwordHasher.HashPassword(command.Password).Returns(hashedPassword);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<CreateUserResult>(Arg.Any<User>()).Returns(result);

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
        
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).HashPassword(command.Password);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateUserResult>(Arg.Any<User>());
    }

    /// <summary>
    /// Tests that an invalid user creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateUserCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that creating a user with existing email throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given existing email When creating user Then throws invalid operation exception")]
    public async Task Handle_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var existingUser = User.Create(
            "existinguser",
            command.Email,
            "+5511999999999",
            "HashedPass123!",
            UserRole.Customer
        );

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"User with email {command.Email} already exists");
    }

    /// <summary>
    /// Tests that the password is hashed before saving the user.
    /// </summary>
    [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var originalPassword = command.Password;
        const string hashedPassword = "h@shedPassw0rd";
        var user = User.Create(
            command.Username,
            command.Email,
            command.Phone,
            hashedPassword,
            command.Role
        );

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);
        _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(new CreateUserResult { Id = user.Id });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _passwordHasher.Received(1).HashPassword(originalPassword);
        await _userRepository.Received(1).CreateAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the unit of work is committed after user creation.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then commits unit of work")]
    public async Task Handle_ValidRequest_CommitsUnitOfWork()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = User.Create(
            command.Username,
            command.Email,
            command.Phone,
            "HashedPass123!",
            command.Role
        );

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("HashedPass123!");
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(new CreateUserResult { Id = user.Id });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
