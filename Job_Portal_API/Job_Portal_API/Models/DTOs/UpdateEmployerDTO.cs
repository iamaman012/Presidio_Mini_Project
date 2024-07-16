namespace Job_Portal_API.Models.DTOs
{
    public class UpdateEmployerDTO
    {
        public int EmployerID { get; set; }
        public String CompanyName { get; set; }
        public String CompanyDescription { get; set; }
        public String CompanyLocation { get; set; }

    }
}
