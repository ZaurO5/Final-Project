using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.ViewModels.ContactMessage;
using Data.Contexts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ContactUsController : Controller
{
    private readonly AppDbContext _context;
    private const int PageSize = 10;

    public ContactUsController(AppDbContext context)
    {
        _context = context;
    }

    #region Read

    public async Task<IActionResult> Index(int page = 1)
    {
        var query = _context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ContactMessageListVM
            {
                Id = m.Id,
                CreatedAt = m.CreatedAt,
                Name = m.Name,
                Email = m.Email,
                Content = m.Content,
                IsRead = m.IsRead
            });

        var totalMessages = await query.CountAsync();
        var messages = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalMessages / (double)PageSize);

        return View(messages);
    }

    #endregion

    #region ToggleReadStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleReadStatus(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null) return NotFound();

        message.IsRead = !message.IsRead;
        await _context.SaveChangesAsync();

        return Ok(new { newStatus = message.IsRead });
    }

    #endregion

    #region Delete

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null) return NotFound();

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync();

        return Ok();
    }

    #endregion
}
