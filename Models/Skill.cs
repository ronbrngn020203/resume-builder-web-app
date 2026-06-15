using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace ResumeBuilderWebApp.Models
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        [Required(ErrorMessage = "Skill name is required.")]
        [StringLength(50)]
        public string SkillName { get; set; }

        [Required(ErrorMessage = "Proficiency is required.")]
        [RegularExpression(@"^(Beginner|Intermediate|Expert)$", ErrorMessage = "Choose a valid proficiency level.")]
        public string Proficiency { get; set; }

        [ForeignKey("Resume")]
        public int ResumeId { get; set; }

        [ValidateNever]
        public Resume Resume { get; set; }
    }
}
