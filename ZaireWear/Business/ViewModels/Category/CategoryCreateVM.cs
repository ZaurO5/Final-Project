using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "At least 3 characters")]
        public string Name { get; set; }
    }
}
