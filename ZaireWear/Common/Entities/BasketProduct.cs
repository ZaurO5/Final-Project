using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class BasketProduct : BaseEntity
{
    public int BasketId { get; set; }
    public Basket Basket { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
}
