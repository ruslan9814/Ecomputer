using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ProductReviews.Requests;

public sealed record UpdateProductReviewRequest(
    int Id,
    string? Content = null,
    int? Rating = null);
