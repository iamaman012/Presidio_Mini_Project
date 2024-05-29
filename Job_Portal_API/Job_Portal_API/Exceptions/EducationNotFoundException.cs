using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class EducationNotFoundException : Exception
    {   
        public string message;
        public EducationNotFoundException()
        {
            message = "Education not found in the database";
        }

        public EducationNotFoundException(string? message) 
        { this.message = message; }

        public override string Message => message;  

    }

        
    
}