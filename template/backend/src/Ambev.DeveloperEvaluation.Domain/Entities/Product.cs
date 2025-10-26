using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    public int ProductNumber { get; protected set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string Category { get; private set; }
    public string ImageUrl { get; private set; }
    public Rating Rating { get; private set; }

#pragma warning disable CS8618 // Supressed to allow EF Core materialization
    protected Product() { }
#pragma warning restore CS8618

    public Product(
        string title,
        string description,
        decimal price,
        string category,
        string imageUrl,
        Rating rating)
    {
        Title = title;
        Description = description;
        Price = price;
        Category = category;
        ImageUrl = imageUrl;
        Rating = rating;
        CreatedAt = DateTime.UtcNow;
    }

    public static Product Create(
        string title,
        string description,
        decimal price,
        string category,
        string imageUrl,
        decimal rate,
        int rateCount)
    {
        return price <= 0
                ? throw new ArgumentOutOfRangeException("Price must be greater than zero")
                : new(title, description, price, category, imageUrl, new(rate, rateCount));
    }

    public void Update(
        string title,
        string description,
        decimal price,
        string category,
        string imageUrl,
        decimal rate,
        int rateCount)
    {
        Title = title;
        Description = description;
        Price = price;
        Category = category;
        ImageUrl = imageUrl;
        Rating.Rate = rate;
        Rating.Count = rateCount;
        UpdatedAt = DateTime.UtcNow;
    }
}

[Owned]
public class Rating
{
    public decimal Rate { get; internal set; }
    public int Count { get; internal set; }

    public Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }
}
