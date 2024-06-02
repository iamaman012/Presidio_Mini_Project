using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class JobSeekerRepository : IRepository<int, JobSeeker>
    {
        private readonly JobPortalApiContext _context;

        public JobSeekerRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<JobSeeker> Add(JobSeeker entity)
        {

            var jobSeeker = _context.JobSeekers.Where(js => js.UserID == entity.UserID);
            if(jobSeeker!=null)
            {
                throw new JobSeeKerAlreadyExistExceptiom("JOb Seeker Alread Exist");
            }
            await _context.JobSeekers.AddAsync(entity);
            await _context.SaveChangesAsync();
            var jobSeeker= await _context.JobSeekers.FirstOrDefaultAsync(js=>js.UserID==entity.UserID);
            return jobSeeker;
        }

        public async Task<JobSeeker> Update(JobSeeker entity)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(entity.JobSeekerID);
            if (jobSeeker == null)
            {
                throw new UserNotFoundException("Job Seeker Not Found");
            }
            _context.JobSeekers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobSeeker> DeleteById(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                throw new UserNotFoundException("Job Seeker Not Found");
            }
            _context.JobSeekers.Remove(jobSeeker);
            await _context.SaveChangesAsync();
            return jobSeeker;
        }

        public async Task<JobSeeker> GetById(int id)
        {
            var jobSeeker = await _context.JobSeekers
                .Include(js => js.User)
                .Include(js => js.JobSeekerSkills)
                .Include(js => js.JobSeekerEducations)
                .Include(js => js.JobSeekerExperiences)
                .FirstOrDefaultAsync(js => js.JobSeekerID == id);

            if (jobSeeker == null)
            {
                throw new JobSeekerNotFoundException();
            }
            return jobSeeker;
        }

        public async Task<IEnumerable<JobSeeker>> GetAll()
        {
            var jobSeekers = await _context.JobSeekers
               .Include(js => js.User)
               .Include(js => js.JobSeekerSkills)
               .Include(js => js.JobSeekerEducations)
               .Include(js => js.JobSeekerExperiences).ToListAsync();
            return jobSeekers;
        }
    }
}
