using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.ContactMessage;
public class ContactMessageListVM
{
    public int Id { get; set; }
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(500)]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
