using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Favorites.Responses;

public sealed record FavoriteResponse(
    int Id,
    int UserId,
    int ProductId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
