using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Orders.Responses;

public sealed record OrderResponse(
    int Id,
    string UserId,
    string Address,
    string PhoneNumber,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    decimal TotalPrice,
    bool IsPaid,
    bool IsShipped,
    bool IsCompleted);