using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class JobSeekerSkillRepository : IRangeRepository<int, JobSeekerSkill>
    {
        private readonly JobPortalApiContext _context;

        public JobSeekerSkillRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerSkill> Add(JobSeekerSkill entity)
        {
            await _context.JobSeekerSkills.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerSkill> Update(JobSeekerSkill entity)
        {
            var jobSeekerSkill = await _context.JobSeekerSkills.FindAsync(entity.JobSeekerSkillID);
            if (jobSeekerSkill == null)
            {
                throw new JobSeekerSkillNotFoundException();
            }
            _context.JobSeekerSkills.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerSkill> DeleteById(int id)
        {
            var jobSeekerSkill = await _context.JobSeekerSkills.FindAsync(id);
            if (jobSeekerSkill == null)
            {
                throw new JobSeekerSkillNotFoundException();
            }
            _context.JobSeekerSkills.Remove(jobSeekerSkill);
            await _context.SaveChangesAsync();
            return jobSeekerSkill;
        }

        public async Task<JobSeekerSkill> GetById(int id)
        {
            var jobSeekerSkill = await _context.JobSeekerSkills.FindAsync(id);
            if (jobSeekerSkill == null)
            {
                throw new JobSeekerSkillNotFoundException();
            }
            return jobSeekerSkill;
        }

        public async Task<IEnumerable<JobSeekerSkill>> GetAll()
        {
            return await _context.JobSeekerSkills.ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerSkill>> AddRange(IEnumerable<JobSeekerSkill> entities)
        {
            // Add a collection of JobSeekerSkill entities to the database
            // and save changes asynchronously
            await _context.JobSeekerSkills.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            // Return the added entities
            return entities;
        }
    }
}
