using System;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderWebApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        [StringLength(50)]
        public string Role { get; set; } = "Graduate";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
