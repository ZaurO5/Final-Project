using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Size
{
    public class SizeCreateVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
