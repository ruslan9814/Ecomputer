using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Promotions;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive => DateTime.UtcNow >= ValidFrom && DateTime.UtcNow <= ValidTo;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public Coupon() { }
    public Coupon(int id, string code, decimal discountAmount, DateTime validFrom, DateTime validTo)
    {
        Id = id;
        Code = code;
        DiscountAmount = discountAmount;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
}
