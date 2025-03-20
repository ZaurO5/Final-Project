using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class ContactMessage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Message content is required")]
    [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}