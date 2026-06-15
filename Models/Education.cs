using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ResumeBuilderWebApp.Models
{
    public class Education
    {
        [Key]
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Institution is required.")]
        [Display(Name = "Institution Name")]
        [StringLength(100)]
        public string Institution { get; set; } = string.Empty;
        [Required(ErrorMessage = "Degree field is required.")]
        public string Degree { get; set; } = string.Empty;

        [StringLength(100)]
        public string FieldOfStudy { get; set; }

        [Required(ErrorMessage = "Start date required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date required.")]
        [DateGreaterThan("StartDate", ErrorMessage = "End Date must be after Start Date.")]
        public DateTime EndDate { get; set; }

        [ForeignKey("Resume")]
        public int ResumeId { get; set; }

        [ValidateNever]
        public Resume Resume { get; set; }
    }
}
