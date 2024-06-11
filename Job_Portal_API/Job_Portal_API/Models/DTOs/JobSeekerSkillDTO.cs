using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal_API.Models.DTOs
{
    public class JobSeekerSkillDTO
    {
        [Required(ErrorMessage = "JobSeekerID is required")]
        public int JobSeekerID { get; set; }

        [Required(ErrorMessage = "SkillNames is required")]
        public List<string> SkillNames { get; set; }
    }
}
