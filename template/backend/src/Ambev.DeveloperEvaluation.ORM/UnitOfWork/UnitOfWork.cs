using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public IProductRepository Products { get; }
    public IUserRepository Users { get; }
    public IOrderRepository Orders { get; }
    public IBranchRepository Branches { get; }

    public UnitOfWork(
        DbContext context,
        IProductRepository productRepository,
        IUserRepository users,
        IOrderRepository orders,
        IBranchRepository branches)
    {
        _context = context;
        Products = productRepository;
        Users = users;
        Orders = orders;
        Branches = branches;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
