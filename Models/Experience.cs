using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ResumeBuilderWebApp.Models
{
    public class Experience
    {
        [Key]
        public int ExperienceId { get; set; }
        [Required(ErrorMessage = "Company name required.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Position required.")]
        public string Position { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be after Start Date.")]
        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey("Resume")]
        public int ResumeId { get; set; }

        [ValidateNever]
        public Resume Resume { get; set; }
    }
}
