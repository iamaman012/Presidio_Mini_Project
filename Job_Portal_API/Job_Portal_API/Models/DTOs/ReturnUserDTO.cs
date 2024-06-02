namespace Job_Portal_API.Models.DTOs
{
    public class ReturnUserDTO
    {
        public int UserID { get; set; } 

        public int ?JobSeekerID { get; set; }

        public string Email { get; set; }   
        public string Name { get; set; }    

        public string ContactNumber { get; set; }
        public string Role { get; set; }
    }
}
