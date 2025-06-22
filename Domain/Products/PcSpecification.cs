 

namespace Domain.Products;

public class PcSpecification : EntityBase
{
    public double CpuGhz { get; set; }
    public int RamGb { get; set; }
    public float GpuScore { get; set; }
    public int SsdGb { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public PcSpecification(double cpuGhz, int ramGb, float gpuScore, int ssdGb, Product product) : base(0)
    {
        CpuGhz = cpuGhz;
        RamGb = ramGb;
        GpuScore = gpuScore;
        SsdGb = ssdGb;
        Product = product;
        ProductId = product.Id;
    }
}

