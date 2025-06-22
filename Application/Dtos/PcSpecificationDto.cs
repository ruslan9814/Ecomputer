using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos;

public sealed record PcSpecificationDto(
    float CpuGhz,
    int RamGb,
    float GpuScore,
    int SsdGb
);

