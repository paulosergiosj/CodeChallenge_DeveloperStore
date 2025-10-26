using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public IProductRepository Products { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(
        DbContext context,
        IProductRepository productRepository,
        IUserRepository users)
    {
        _context = context;
        Products = productRepository;
        Users = users;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
