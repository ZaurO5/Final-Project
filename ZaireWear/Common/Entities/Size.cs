using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Size : BaseEntity
{
    public string Name { get; set; }
    public ICollection<ProductSizes> ProductSizes { get; set; }
}
