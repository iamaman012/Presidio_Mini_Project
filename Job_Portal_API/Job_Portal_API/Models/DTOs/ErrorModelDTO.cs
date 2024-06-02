namespace Job_Portal_API.Models.DTOs
{
    public class ErrorModelDTO
    {   
        public ErrorModelDTO(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        
        public int StatusCode { get; set; } 
        public string Message { get; set; }

    }
}
