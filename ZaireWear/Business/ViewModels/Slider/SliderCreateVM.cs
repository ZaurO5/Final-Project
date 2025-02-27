using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(5, ErrorMessage = "At least 5 characters")]
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Order must be at least 1")]
        public int Order { get; set; }
    }
}
