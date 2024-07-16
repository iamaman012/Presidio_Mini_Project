using Job_Portal_API.Context;
using Job_Portal_API.Exceptions;
using Job_Portal_API.Interfaces;
using Job_Portal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal_API.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly JobPortalApiContext _context;

        public UserRepository(JobPortalApiContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User entity)
        {
            var user =  _context.Users.FirstOrDefault(u => u.Email == entity.Email);
            if(user != null)
            {
                throw new UserAlreadyExistException("Email is Already Exist!!");
            }
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            var result =  _context.Users.FirstOrDefault(u => u.Email == entity.Email);
            return result;
        }

        public async Task<User> Update(User entity)
        {
            var user =   _context.Users.Find(entity.UserID);
            if(user==null)
            {
                throw new UserNotFoundException();
            }
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> DeleteById(int id)
        {
            var user = await _context.Users
                                     .Include(u => u.Employer) // Include Employer first
                                     .Include(u => u.JobSeeker)
                                     .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                throw new UserNotFoundException("User not found!");
            }

            if (user.Employer != null)
            {
                // Load JobListings separately if Employer is not null
                var employer = _context.Employers
                                          .Include(e => e.JobListings)
                                          .ThenInclude(j => j.Applications)
                                          .FirstOrDefault(e => e.EmployerID == user.Employer.EmployerID);
                _context.Applications.RemoveRange(employer.JobListings.SelectMany(j => j.Applications));
                _context.JobListings.RemoveRange(employer.JobListings);
                _context.Employers.Remove(employer);

            }
            if(user.JobSeeker!=null)
            {
                var jobSeeker = _context.JobSeekers
                    .Include(j => j.Applications)
                    .FirstOrDefault(j => j.JobSeekerID == user.JobSeeker.JobSeekerID);
                _context.Applications.RemoveRange(jobSeeker.Applications);
                _context.JobSeekers.Remove(jobSeeker);

            }
           

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetById(int id)
        {
            var user= await _context.Users.FindAsync(id);
            if (user==null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var results = await _context.Users.Include(u=>u.JobSeeker).Include(u=>u.Employer).ToListAsync();
            return results;

        }   
    }
}
