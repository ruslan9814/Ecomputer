using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Orders.Requests;

public sealed record UpdateOrderStatusRequest(sbyte Status);
