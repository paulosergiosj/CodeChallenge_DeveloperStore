using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Branch : BaseEntity
{
    public string Name { get; private set; }

    public Branch(string name)
    {
        Name = name;
    }

    //Seeding constructor
    public Branch(Guid id, string name)
    {
        Id = id;
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    public static Branch Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Branch name cannot be empty.");
        }

        return new(name);
    }
}
