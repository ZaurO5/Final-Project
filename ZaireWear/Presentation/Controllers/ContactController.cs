using Core.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                message.CreatedAt = DateTime.UtcNow;
                message.IsRead = false;
                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Message sent successfully!";
                return RedirectToAction("Index", "Home");
            }
            return View("Index", message);
        }
    }

}
