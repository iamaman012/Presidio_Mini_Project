using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class JobListingRepository : IRepository<int, JobListing>
    {
        private readonly JobPortalApiContext _context;

        public JobListingRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<JobListing> Add(JobListing entity)
        {

            await _context.JobListings.AddAsync(entity);
            await _context.SaveChangesAsync();
            var result = await _context.JobListings
               .Include(jl => jl.JobSkills)
               .OrderByDescending(jl => jl.JobID)
               .FirstOrDefaultAsync();
            


            //foreach (var skill in result.JobSkills)
            //{
            //    skill.JobID = result.JobID;
            //    await _context.JobSkills.AddAsync(skill);
            //    await _context.SaveChangesAsync();
            //}
            return result;
           

           
        }


        public async Task<JobListing> Update(JobListing entity)
        {
            var jobListing = await _context.JobListings
                 .Include(jl => jl.JobSkills)
                 .FirstOrDefaultAsync(jl => jl.JobID == entity.JobID);
            if (jobListing == null)
            {
                throw new JobListingNotFoundException();
            }

            _context.Entry(jobListing).CurrentValues.SetValues(entity);
            jobListing.JobSkills.Clear();
            jobListing.JobSkills = entity.JobSkills;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<JobListing> DeleteById(int id)
        {
            var jobListing = await _context.JobListings
                .Include(jl => jl.JobSkills)
                .FirstOrDefaultAsync(jl => jl.JobID == id);
            if (jobListing == null)
            {
                throw new JobListingNotFoundException();
            }
            _context.JobListings.Remove(jobListing);
            await _context.SaveChangesAsync();
            return jobListing;
        }

        public async Task<JobListing> GetById(int id)
        {
            var jobListing = await _context.JobListings
                .Include(jl => jl.JobSkills)
                .FirstOrDefaultAsync(jl => jl.JobID == id);
            if (jobListing == null)
            {
                throw new JobListingNotFoundException();
            }
            return jobListing;
        }

        public async Task<IEnumerable<JobListing>> GetAll()
        {
            var results = await _context.JobListings
                .Include(jl => jl.JobSkills)
                .ToListAsync();
            return results;
        }
    }
}
