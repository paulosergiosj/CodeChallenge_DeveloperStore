using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ambev.DeveloperEvaluation.ORM.Seed;

public static class BranchDataSeed
{
    public static void Seed(EntityTypeBuilder<Branch> modelBuilder)
    {
        modelBuilder.HasData(
            new Branch(Guid.NewGuid(), "branch A"),
            new Branch(Guid.NewGuid(), "Branch B")
        );
    }
}
