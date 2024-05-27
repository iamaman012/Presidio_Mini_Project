using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Context
{
    public class JobPortalApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=71RBBX3\\SQLEXPRESS;Integrated Security=true;Initial Catalog=db_Job_Portal_API;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }
        public DbSet<JobSeekerExperience> Experiences { get; set; }
        public DbSet<JobSeekerEducation> Educations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Employer>().HasKey(e => e.EmployerID);
            modelBuilder.Entity<JobSeeker>().HasKey(j => j.JobSeekerID);
            modelBuilder.Entity<JobListing>().HasKey(j => j.JobID);
            modelBuilder.Entity<Application>().HasKey(a => a.ApplicationID);
            modelBuilder.Entity<Skill>().HasKey(s => s.SkillID);
            modelBuilder.Entity<JobSeekerExperience>().HasKey(e => e.ExperienceID);
            modelBuilder.Entity<JobSeekerEducation>().HasKey(e => e.EducationID);
            modelBuilder.Entity<JobSeekerEducation>().HasKey(e => e.EducationID);
            modelBuilder.Entity<JobSkill>().HasKey(js => new { js.JobID, js.SkillID });
            modelBuilder.Entity<JobSeekerSkill>().HasKey(js => new { js.JobSeekerID, js.SkillID });

            modelBuilder.Entity<Skill>()
                .HasData(new Skill {SkillID=101, SkillName = "C#" });

            modelBuilder.Entity<Employer>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employer)
                .HasForeignKey<Employer>(e => e.UserID);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            
            modelBuilder.Entity<JobSeeker>()
                .HasOne(j => j.User)
                .WithOne(u => u.JobSeeker)
                .HasForeignKey<JobSeeker>(j => j.UserID);

            modelBuilder.Entity<JobListing>()
                .HasOne(j => j.Employer)
                .WithMany(e => e.JobListings)
                .HasForeignKey(j => j.EmployerID);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeeker)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobSeekerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobListing)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobListing>()
                .HasMany(j => j.JobSkills)
                .WithOne(js => js.JobListing)
                .HasForeignKey(js => js.JobID);

            modelBuilder.Entity<JobSeeker>()
                .HasMany(j => j.JobSeekerSkills)
                .WithOne(js => js.JobSeeker)
                .HasForeignKey(js => js.JobSeekerID);


            modelBuilder.Entity<Skill>()
                .HasMany(s => s.JobSkills)
                .WithOne(js => js.Skill)
                .HasForeignKey(js => js.SkillID);

            modelBuilder.Entity<Skill>()
                .HasMany(s => s.JobSeekerSkills)
                .WithOne(js => js.Skill)
                .HasForeignKey(js => js.SkillID);

           modelBuilder.Entity<JobSeekerExperience>()
                .HasOne(j => j.JobSeeker)
                .WithMany(j => j.JobSeekerExperiences)
                .HasForeignKey(j => j.JobSeekerID);

            modelBuilder.Entity<JobSeekerEducation>()
                .HasOne(j => j.JobSeeker)
                .WithMany(j => j.JobSeekerEducations)
                .HasForeignKey(j => j.JobSeekerID);



        }
    }
}
