using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Job_Portal_API.Models.DTOs;
using Job_Portal_API.Models.Enums;
using Job_Portal_API.Repositories;
using Job_Portal_API.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesTest
{
    public class JobSeekerServiceTests
    {
        private JobPortalApiContext _context;
        private IRepository<int, JobSeeker> _jobSeekerRepo;
        private IRepository<int, JobSeekerExperience> _expRepo;
        private IRepository<int, JobSeekerEducation> _eduRepo;
        private IRangeRepository<int, JobSeekerSkill> _skillRepo;
        private IJobSeeker _jobSeekerService;

        [SetUp]
        public async  Task Setup()
        {
            var options = new DbContextOptionsBuilder<JobPortalApiContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _context = new JobPortalApiContext(options);
            _jobSeekerRepo = new JobSeekerRepository(_context);
            _expRepo = new ExperienceRepository(_context);
            _eduRepo = new EducationRepository(_context);
            _skillRepo = new JobSeekerSkillRepository(_context);
            _jobSeekerService = new JobSeekerService(_jobSeekerRepo, _expRepo, _eduRepo, _skillRepo);
            var user1 = new User
            {
                UserID = 1,
                Email = "abc@gmail.com",
                FirstName = "abc",
                LastName = "xyz",
                ContactNumber = "1234567890",
                UserType = UserType.JobSeeker,
                DateOfRegistration = DateTime.Now,
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 5, 6, 7, 8 }
            };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            var jobSeeker = new JobSeeker { JobSeekerID = 1, UserID = 1 };
            await _context.JobSeekers.AddAsync(jobSeeker);
            await _context.SaveChangesAsync();

            var skill1 = new JobSeekerSkill { JobSeekerSkillID = 1, JobSeekerID = 1, SkillName = "Skill A" };
            await _context.JobSeekerSkills.AddAsync(skill1);
            await _context.SaveChangesAsync();

            var education1 = new JobSeekerEducation { EducationID = 1, JobSeekerID = 1, Degree = "Bachelor's", Institution = "University A", Description = "dsdd", Location = "dwsd" };
            await _context.Educations.AddAsync(education1);
            await _context.SaveChangesAsync();

            var experience1 = new JobSeekerExperience { ExperienceID = 1, JobSeekerID = 1, JobTitle = "Developer", Description = "dsdd", Location = "dwsd", CompanyName = "ABC" };
            await _context.Experiences.AddAsync(experience1);
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddExp_Pass()
        {
           //Arrange

            var expDTO = new ExperienceDTO
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Corp",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now,
                Description = "Developing software"
            };

            var result = await _jobSeekerService.AddExperience(expDTO);

            Assert.IsNotNull(result);
            Assert.AreEqual(expDTO.JobTitle, result.JobTitle);
            
        }

        [Test]
        public void AddExp_Fail_JobSeekerNotExist()
        {
            var expDTO = new ExperienceDTO
            {
                JobSeekerID = 999,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Corp",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now,
                Description = "Developing software"
            };

            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.AddExperience(expDTO));
        }

      

        [Test]
        public async Task AddEdu_Pass()
        {
           
            var eduDTO = new EducationDTO
            {
                JobSeekerID = 1,
                Degree = "B.Sc. Computer Science",
                Institution = "XYZ University",
                Location = "Boston",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };

            var result = await _jobSeekerService.AddEducation(eduDTO);

            Assert.IsNotNull(result);
            Assert.AreEqual(eduDTO.Degree, result.Degree);
        }

        [Test]
        public void AddEdu_Fail_JobSeekerNotExist()
        {
            var eduDTO = new EducationDTO
            {
                JobSeekerID = 999,
                Degree = "B.Sc. Computer Science",
                Institution = "XYZ University",
                Location = "Boston",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };

            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.AddEducation(eduDTO));
        }

      

        [Test]
        public async Task GetResume_Pass()
        {
           
            //Arrange
            var result = await _jobSeekerService.GetResumeByJobSeekerId(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.JobSeekerID);
        }

        [Test]
        public void GetResume_Fail_JobSeekerNotExist()
        {
            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.GetResumeByJobSeekerId(999));
        }

       

        [Test]
        public async Task AddSkills_Pass()
        {
           
            var skillDto = new JobSeekerSkillDTO
            {
                JobSeekerID = 1,
                SkillNames = new List<string> { "C#", "SQL" }
            };

            var result = await _jobSeekerService.AddJobSeekerSkillsAsync(skillDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void AddSkills_Fail_JobSeekerNotExist()
        {
            var skillDto = new JobSeekerSkillDTO
            {
                JobSeekerID = 999,
                SkillNames = new List<string> { "C#", "SQL" }
            };

            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.AddJobSeekerSkillsAsync(skillDto));
        }

     

        [Test]
        public async Task DelExp_Pass()
        {
            
            var exp = new JobSeekerExperience
            {
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Corp",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now,
                Description = "Developing software"
            };
            await _expRepo.Add(exp);

            var result = await _jobSeekerService.DeleteExperirnceById(exp.ExperienceID);

            Assert.NotNull(result);
        }

        [Test]
        public void DelExp_Fail_ExpNotExist()
        {
            Assert.ThrowsAsync<ExperienceNotFoundException>(async () => await _jobSeekerService.DeleteExperirnceById(999));
        }

     

        [Test]
        public async Task DelEdu_Pass()
        {
            
            var edu = new JobSeekerEducation
            {
                JobSeekerID = 1,
                Degree = "B.Sc. Computer Science",
                Institution = "XYZ University",
                Location = "Boston",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };
            await _eduRepo.Add(edu);

            var result = await _jobSeekerService.DeleteEducationById(edu.EducationID);

            Assert.NotNull(result);
        }

        [Test]
        public void DelEdu_Fail_EduNotExist()
        {
            Assert.ThrowsAsync<EducationNotFoundException>(async () => await _jobSeekerService.DeleteEducationById(999));
        }
        [Test]
        public async Task DelSkillByID_Pass()
        {
            //Arrange
            var skill = new JobSeekerSkill
            {
                JobSeekerID = 1,
                SkillName = "C#"
            };
            await _skillRepo.Add(skill);

            var result = await _jobSeekerService.DeleteSkillById(skill.JobSeekerSkillID);

            Assert.NotNull(result);
            Assert.AreEqual(result.SkillName, skill.SkillName);
        }

        [Test]
        public async Task DelSkillByID_Fail()
        {
            Assert.ThrowsAsync<JobSeekerSkillNotFoundException>(async () => await _jobSeekerService.DeleteSkillById(999));
        }

        [Test]
        public async Task UpdateEducation_Pass()
        {
            //Arrange
            var education = new JobSeekerEducation {  JobSeekerID = 1, Degree = "Bachelor's", Institution = "University A", Description = "dsdd", Location = "dwsd" };
            await _context.Educations.AddAsync(education);
            await _context.SaveChangesAsync();

            var eduDTO = new EducationResponseDTO
            {
                EducationID = education.EducationID,
                JobSeekerID = 1,
                Degree = "Master's",
                Institution = "University B",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };
           var result = await _jobSeekerService.UpdateEducation(eduDTO);

            Assert.IsNotNull(result);
            Assert.AreEqual(eduDTO.Degree, result.Degree);
            Assert.AreEqual(eduDTO.EducationID, result.EducationID  );
        }

        [Test]
        public async Task UpdateEducation_EduExcep_Pass()
        { //Arrange
            var eduDTO = new EducationResponseDTO
            {
                EducationID = 999,
                JobSeekerID = 1,
                Degree = "Master's",
                Institution = "University B",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };
            Assert.ThrowsAsync<EducationNotFoundException>(async () => await _jobSeekerService.UpdateEducation(eduDTO));
        }
        [Test]
        public async Task UpdateEducation_JobSeekerExcep_Pass()
        { //Arrange
            var eduDTO = new EducationResponseDTO
            {
                EducationID = 1,
                JobSeekerID = 999,
                Degree = "Master's",
                Institution = "University B",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now,
                Description = "Studied computer science",
                GPA = 3.8
            };
            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.UpdateEducation(eduDTO));
        }
        [Test]
        public async Task UpdateExperience_Pass()
        {
            //Arrange
            var experience1 = new JobSeekerExperience { JobSeekerID = 1, JobTitle = "Developer", Description = "dsdd", Location = "dwsd", CompanyName = "ABC" };
            await _context.Experiences.AddAsync(experience1);
            await _context.SaveChangesAsync();
           

           var expDto = new ExperienceResponseDTO
           {
               ExperienceID = experience1.ExperienceID,
               JobSeekerID = 1,
               JobTitle = "Software Engineer",
               CompanyName = "Tech Corp",
               Location = "New York",
               StartDate = DateTime.Now.AddYears(-2),
               EndDate = DateTime.Now,
               Description = "Developing software"
           };

            var result = await _jobSeekerService.UpdateExperience(expDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(expDto.JobTitle, result.JobTitle);
            Assert.AreEqual(expDto.ExperienceID, result.ExperienceID);
        }

        [Test]
        public async Task UpdateExperinece_ExpExcep_Pass()
        { //Arrange

            var expDto = new ExperienceResponseDTO
            {
                ExperienceID = 999,
                JobSeekerID = 1,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Corp",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now,
                Description = "Developing software"
            };

            Assert.ThrowsAsync<ExperienceNotFoundException>(async () => await _jobSeekerService.UpdateExperience(expDto));
        }
        [Test]
        public async Task UpdateExp_JobSeekerExcep_Pass()
        { //Arrange

            var expDto = new ExperienceResponseDTO
            {
                ExperienceID = 1,
                JobSeekerID = 999,
                JobTitle = "Software Engineer",
                CompanyName = "Tech Corp",
                Location = "New York",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now,
                Description = "Developing software"
            };

            Assert.ThrowsAsync<JobSeekerNotFoundException>(async () => await _jobSeekerService.UpdateExperience(expDto));
        }

    }
}
