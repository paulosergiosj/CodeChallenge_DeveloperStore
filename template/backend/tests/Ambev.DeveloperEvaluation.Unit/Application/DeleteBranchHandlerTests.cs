using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for DeleteBranchHandler.
/// </summary>
public class DeleteBranchHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBranchRepository _branchRepository;
    private readonly DeleteBranchHandler _handler;

    public DeleteBranchHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _branchRepository = Substitute.For<IBranchRepository>();

        _unitOfWork.Branches.Returns(_branchRepository);

        _handler = new DeleteBranchHandler(_unitOfWork);
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then returns success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);
        var expectedResult = DeleteBranchHandlerTestData.GenerateValidResult();

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Branch deleted successfully");
    }

    [Fact(DisplayName = "Given invalid command When deleting branch Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateInvalidCommand();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Errors.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Given non-existent branch When deleting branch Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentBranch_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Branch with ID {command.BranchId} not found");
    }

    [Fact(DisplayName = "Given branch referenced in orders When deleting branch Then throws InvalidOperationException")]
    public async Task Handle_BranchReferencedInOrders_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Cannot delete branch {branch.Name} because it is referenced in existing orders");
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then calls FirstOrDefaultAsync")]
    public async Task Handle_ValidCommand_CallsFirstOrDefaultAsync()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _branchRepository.Received(1).FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then calls IsReferencedInOrdersAsync")]
    public async Task Handle_ValidCommand_CallsIsReferencedInOrdersAsync()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _branchRepository.Received(1).IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then calls Remove")]
    public async Task Handle_ValidCommand_CallsRemove()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _branchRepository.Received(1).Remove(branch);
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then commits unit of work")]
    public async Task Handle_ValidCommand_CommitsUnitOfWork()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When deleting branch Then returns correct result structure")]
    public async Task Handle_ValidCommand_ReturnsCorrectResultStructure()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = DeleteBranchHandlerTestData.GenerateValidBranch(command.BranchId);

        _branchRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Branch, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(branch);
        _branchRepository.IsReferencedInOrdersAsync(command.BranchId, Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<DeleteBranchResult>();
        result.Success.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
    }
}
