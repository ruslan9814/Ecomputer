using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ProductReviews.Responses;

public sealed record ProductReviewResponse(
    int Id,
    int ProductId,
    int UserId,
    string UserFullName,
    string ReviewText,
    int Rating,
    DateTime CreatedAt);

