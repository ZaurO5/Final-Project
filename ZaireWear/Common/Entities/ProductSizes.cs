using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class ProductSizes
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int SizeId { get; set; }
    public Size Size { get; set; }
}