﻿using Job_Portal_API.Models.Enums;

namespace Job_Portal_API.Models.DTOs
{
    public class JobListingResponseDTO
    {
        public int JobID { get; set; }
        public string JobTitle { get; set; }

        
        public string JobDescription { get; set; }
        public string JobType { get; set; }
        public string Location { get; set; }
        public double Salary { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }

        public string? CompanyLocation { get; set; }
        public int? EmployerID { get; set; }
        public List<JobSkillResponseDTO>? Skills { get; set; }

    }
}
