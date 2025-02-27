using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ZaireWear.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;

        public HomeController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _sliderService.GetAllAsync();
                if (model == null)
                {
                    return View("Error");
                }

                model.Sliders = model.Sliders.Where(s => s.IsActive).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
