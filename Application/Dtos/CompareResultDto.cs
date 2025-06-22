using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos;

public record CompareResultDto(
    string CpuBetter,
    string RamBetter,
    string GpuBetter,
    string SsdBetter,
    string PriceBetter
);

