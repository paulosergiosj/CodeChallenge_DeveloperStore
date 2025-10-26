//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Repositories;
//using MongoDB.Driver;

//namespace Ambev.DeveloperEvaluation.NoSQL.Repositories;
//public class ProductReviewRepository : IProductReviewRepository
//{
//    private readonly IMongoCollection<ProductReview> _reviews;

//    public ProductReviewRepository(IMongoDatabase database)
//    {
//        _reviews = database.GetCollection<ProductReview>("ProductReviews");
//    }

//    public async Task AddReviewAsync(ProductReview review)
//        => await _reviews.InsertOneAsync(review);

//    public async Task<(decimal Average, int Count)> GetAggregatedRatingAsync(Guid productId)
//    {
//        var aggregation = await _reviews.Aggregate()
//            .Match(r => r.ProductRefId == productId)
//            .Group(
//                key => key.ProductRefId,
//                g => new
//                {
//                    Average = g.Average(r => r.Score),
//                    Count = g.Count()
//                }
//            )
//            .FirstOrDefaultAsync();

//        if (aggregation == null)
//        {
//            return (0, 0);
//        }

//        return ((decimal)aggregation.Average, aggregation.Count);
//    }

//    public async Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(Guid productId)
//    {
//        var filter = Builders<ProductReview>.Filter.Eq(r => r.ProductRefId, productId);

//        var reviews = await _reviews
//            .Find(filter)
//            .SortByDescending(r => r.CreatedAt)
//            .ToListAsync();

//        return reviews;
//    }
//}
