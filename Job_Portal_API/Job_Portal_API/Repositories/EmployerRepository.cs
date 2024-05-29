using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class EmployerRepository : IRepository<int, Employer>
    {
        private readonly JobPortalApiContext _context;

        public EmployerRepository(JobPortalApiContext context)
        {
            _context = context;
        }
        public  async Task<Employer> Add(Employer entity)
        {
            try
            {
                await _context.Employers.AddAsync(entity);
                await _context.SaveChangesAsync();
                var result = _context.Employers.FirstOrDefault(e => e.UserID == entity.UserID);
                return result;
            }
            catch (Exception e)
            {
                throw new UserAlreadyExistException("Employer Already Exist");
            }
          
        }

        public  async Task<Employer> DeleteById(int id)
        {
           var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                throw new UserNotFoundException("Employer Not Found In the Database");
            }
            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
            return employer;
        }

        public  async Task<IEnumerable<Employer>> GetAll()
        {
             var employers = await _context.Employers.ToListAsync();
            return employers;
        }

        public async Task<Employer> GetById(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                throw new UserNotFoundException("Employer Not Exist");
            }
            return employer;
        }

        public async Task<Employer> Update(Employer entity)
        {
            var employer = await _context.Employers.FindAsync(entity.EmployerID);
            if (employer == null)
            {
                throw new UserNotFoundException("Employer Not Exist");
            }
            _context.Employers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
