using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class ApplicationRepository : IRepository<int, Application>
    {
        private readonly JobPortalApiContext _context;

        public ApplicationRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<Application> Add(Application entity)
        {
            await _context.Applications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Application> Update(Application entity)
        {
            var application = await _context.Applications.FindAsync(entity.ApplicationID);
            if (application == null)
            {
                throw new ApplicationNotFoundException();
            }
            _context.Applications.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Application> DeleteById(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                throw new ApplicationNotFoundException();
            }
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<Application> GetById(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                throw new ApplicationNotFoundException();
            }
            return application;
        }

        public async Task<IEnumerable<Application>> GetAll()
        {
            return await _context.Applications.Include(app=>app.JobListing).Include(app=>app.JobListing.Employer).ToListAsync();
        }
    }
}
