using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderWebApp.Models
{
    public class ServiceCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}
