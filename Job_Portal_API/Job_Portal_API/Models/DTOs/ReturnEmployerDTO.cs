namespace Job_Portal_API.Models.DTOs
{
    public class ReturnEmployerDTO
    {
        public int EmployerID { get; set; }
        public int UserID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLocation { get; set; }
    }
}
