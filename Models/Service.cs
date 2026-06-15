using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeBuilderWebApp.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Service title is required.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a short description.")]
        [StringLength(200)]
        public string ShortDescription { get; set; }

        [Display(Name = "Full Description")]
        public string? FullDescription { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Range(0, 9999.99, ErrorMessage = "Price must be between 0 and 9999.99.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Duration (minutes)")]
        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes.")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Please provide a thumbnail path.")]
        public string ThumbnailPath { get; set; } = "/images/default.png";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CategoryId")]
        public ServiceCategory? Category { get; set; }
    }
}
