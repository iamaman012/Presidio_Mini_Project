namespace Job_Portal_API.Models.DTOs
{
    public class ReturnLoginDTO
    {
        public int UserID { get; set; }
        public string Email { get; set; }

        public int JobSeekerId { get; set; }    

        public string Role { get; set; }    
        public string Token { get; set; }
    }
}
