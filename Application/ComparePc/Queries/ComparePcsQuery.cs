//using Application.Dtos;
//using Domain.Products;
//using Infrasctructure.Repositories.Interfaces;


//namespace Application.ComparePc.Queries;

//public record ComparePcsByIdsQuery(int Pc1Id, int Pc2Id) : IRequest<Result<CompareResultDto>>;


//public class ComparePcsByIdsQueryHandler(IProductRepository productRepository) 
//    : IRequestHandler<ComparePcsByIdsQuery, Result<CompareResultDto>>
//{
//    private readonly IProductRepository _productRepository = productRepository;

//    public async Task<Result<CompareResultDto>> Handle(ComparePcsByIdsQuery request, CancellationToken cancellationToken)
//    {
//        var pc1 = await _productRepository.GetAsync(request.Pc1Id);
//        var pc2 = await _productRepository.GetAsync(request.Pc2Id);

//        if (pc1 == null || pc2 == null)
//            return Result.Failure<CompareResultDto>("Один из ПК не найден.");

//        var result = ComparePcs(pc1, pc2);

//        return Result.Success(result);
//    }

//    private CompareResultDto ComparePcs(Product pc1, Product pc2)
//    {
//        return new CompareResultDto(
//            CpuBetter: pc1. > pc2.CpuGhz ? "PC1" : (pc1.CpuGhz < pc2.CpuGhz ? "PC2" : "Equal"),
//            RamBetter: pc1.RamGb > pc2.RamGb ? "PC1" : (pc1.RamGb < pc2.RamGb ? "PC2" : "Equal"),
//            GpuBetter: pc1.GpuScore > pc2.GpuScore ? "PC1" : (pc1.GpuScore < pc2.GpuScore ? "PC2" : "Equal"),
//            SsdBetter: pc1.SsdGb > pc2.SsdGb ? "PC1" : (pc1.SsdGb < pc2.SsdGb ? "PC2" : "Equal"),
//            PriceBetter: pc1.Price < pc2.Price ? "PC1" : (pc1.Price > pc2.Price ? "PC2" : "Equal")
//        );
//    }
//}
