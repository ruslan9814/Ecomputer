

namespace Presentation.ProductReviews.Requests;

public sealed record CreateProductReviewRequest(
    int ProductId,
    string ReviewText,
    int Rating);
