namespace Job_Portal_API.Models.DTOs
{
    public class UserAdminResponseDTO
    {
        public int UserID { get; set; }

        

        public string Email { get; set; }
        public string Name { get; set; }

        public string ContactNumber { get; set; }
        public string Role { get; set; }
    }
}
