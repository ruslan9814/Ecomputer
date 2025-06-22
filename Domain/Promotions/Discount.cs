using Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Coupons;

public class Discount : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => StartDate <= DateTime.UtcNow && EndDate >= DateTime.UtcNow;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public Discount() { }
    public Discount(int id, string name, decimal percentage, DateTime startDate, DateTime endDate) : base(id)
    {
        Name = name;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }
}
