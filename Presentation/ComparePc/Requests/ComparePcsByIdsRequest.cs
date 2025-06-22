using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ComparePc.Requests;

public sealed record ComparePcsByIdsRequest(int Pc1Id, int Pc2Id);
