using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.OrderItems.Requests;

public sealed record DeleteOrderItemRequest(int Id);
