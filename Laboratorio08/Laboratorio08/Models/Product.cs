using System;
using System.Collections.Generic;

namespace Laboratorio08.Models;

public partial class Product
{
    public int Productid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
