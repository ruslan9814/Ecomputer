using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Favorites.Requests;

public sealed record AddFavoriteRequest(
    int ProductId
);
