using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Catalog
{
    public class CatalogIndexVM
    {
        public List<Core.Entities.Product> Products { get; set; }
        public List<Core.Entities.Category> Categories { get; set; }
    }
}
