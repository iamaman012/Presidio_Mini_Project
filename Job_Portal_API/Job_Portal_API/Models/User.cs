namespace Job_Portal_API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public byte[] Password { get; set; }
        public byte[] HashKey { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string UserType { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;

        // Navigation properties
        public Employer Employer { get; set; }
        public JobSeeker JobSeeker { get; set; }    
    }
}
