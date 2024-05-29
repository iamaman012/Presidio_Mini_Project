using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class ExperienceRepository : IRepository<int, JobSeekerExperience>
    {
        private readonly JobPortalApiContext _context;

        public ExperienceRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerExperience> Add(JobSeekerExperience entity)
        {
            await _context.Experiences.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerExperience> Update(JobSeekerExperience entity)
        {
            var experience = await _context.Experiences.FindAsync(entity.ExperienceID);
            if (experience == null)
            {
                throw new ExperienceNotFoundException();
            }
            _context.Experiences.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerExperience> DeleteById(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                throw new ExperienceNotFoundException();
            }
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        public async Task<JobSeekerExperience> GetById(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                throw new ExperienceNotFoundException();
            }
            return experience;
        }

        public async Task<IEnumerable<JobSeekerExperience>> GetAll()
        {
            var experineces=  await _context.Experiences.ToListAsync();
            return experineces; 
        }
    }
}
