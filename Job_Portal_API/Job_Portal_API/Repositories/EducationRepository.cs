using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class EducationRepository : IRepository<int, JobSeekerEducation>
    {
        private readonly JobPortalApiContext _context;

        public EducationRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerEducation> Add(JobSeekerEducation entity)
        {
            await _context.Educations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerEducation> Update(JobSeekerEducation entity)
        {
            var education = await _context.Educations.FindAsync(entity.EducationID);
            if (education == null)
            {
                throw new EducationNotFoundException();
            }
            _context.Educations.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeekerEducation> DeleteById(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                throw new EducationNotFoundException();
            }
            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task<JobSeekerEducation> GetById(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                throw new EducationNotFoundException();
            }
            return education;
        }

        public async Task<IEnumerable<JobSeekerEducation>> GetAll()
        {
            return await _context.Educations.ToListAsync();
        }
    }
}
