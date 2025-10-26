using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Messaging.EventHandlers;

public class CartCheckoutEvent : IHandleMessages<CartCheckoutEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IProductReviewRepository _productReviewRepository;
    public CartCheckoutEvent(
        IUnitOfWork unitOfWork
        //IProductReviewRepository productReviewRepository
        )
    {
        _unitOfWork = unitOfWork;
        //_productReviewRepository = productReviewRepository;
    }
    public async Task Handle(CartCheckoutEvent message)
    {
        //var product = await _unitOfWork.Products.GetByIdAsync(message.ProductId)
        //    ?? throw new KeyNotFoundException($"Product with ID {message.ProductId} not found");

        //(decimal average, int count) = await _productReviewRepository.GetAggregatedRatingAsync(message.ProductId);

        //product.UpdateRating(average, count);

        //await _unitOfWork.CommitAsync();
    }
}
