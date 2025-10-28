using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.UnitOfWork;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }
    IBranchRepository Branches { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
