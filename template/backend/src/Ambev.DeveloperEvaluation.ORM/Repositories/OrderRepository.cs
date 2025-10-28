using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IOrderRepository using Entity Framework Core
/// </summary>
public class OrderRepository : BaseORMRepository<Order>, IOrderRepository
{
    public OrderRepository(DbContext context) : base(context)
    {
    }
}



