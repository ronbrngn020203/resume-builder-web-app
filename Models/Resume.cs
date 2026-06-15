using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderWebApp.Models
{
    public class Resume
    {
        public int ResumeId { get; set; }

        [Required]
        [Display(Name = "Resume Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Full Name must be between 3-60 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "Phone number must contain only digits (8-15 digits).")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Summary too long (max 500 characters).")]
        public string Summary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ Navigation properties for relationships
        public List<Education> Educations { get; set; } = new();
        public List<Experience> Experiences { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}