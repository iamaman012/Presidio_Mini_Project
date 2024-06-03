using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class JobSkillRepository : IRepository<int, JobSkill>
    {
        private readonly JobPortalApiContext _context;

        public JobSkillRepository(JobPortalApiContext context)
        {
            _context = context;
        }
        public async  Task<JobSkill> Add(JobSkill entity)
        {
            await _context.JobSkills.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<JobSkill> DeleteById(int id)
        {
            try
            {
                var jobSkill = await _context.JobSkills.FindAsync(id);
                if (jobSkill == null)
                {
                    throw new JobSkillNotFoundException();
                }
                _context.JobSkills.Remove(jobSkill);
                await _context.SaveChangesAsync();
                return jobSkill;
            }
            catch(JobSkillNotFoundException ex)
            {
                throw new JobSkillNotFoundException(ex.Message);
            }
        }

        public async Task<IEnumerable<JobSkill>> GetAll()
        {
            return await _context.JobSkills.ToListAsync();
        }

        public async Task<JobSkill> GetById(int id)
        {
            try
            {
                var jobSkills= await _context.JobSkills.FindAsync(id);
                if (jobSkills == null)
                {
                    throw new JobSkillNotFoundException();
                }
                return jobSkills;
            }
            catch (JobSkillNotFoundException ex)
            {
                throw new JobSkillNotFoundException(ex.Message);
            }
        }

        public async Task<JobSkill> Update(JobSkill entity)
        {
            try
            {
                var jobSkill = await GetById(entity.JobSkillID);
                _context.JobSkills.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch(JobSkillNotFoundException ex)
            {
                throw new JobSkillNotFoundException(ex.Message);
            }
        }
    }
}
