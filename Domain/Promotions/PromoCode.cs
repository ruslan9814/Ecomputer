using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Coupon;

public class PromoCode : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive => DateTime.UtcNow >= ValidFrom && DateTime.UtcNow <= ValidTo;
    public virtual ICollection<Product> ApplicableProducts { get; set; } = new List<Product>();
    public PromoCode() { }
    public PromoCode(int id, string code, decimal discountPercentage, 
        DateTime validFrom, DateTime validTo) : base(id)
    {
        Code = code;
        DiscountPercentage = discountPercentage;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
}

