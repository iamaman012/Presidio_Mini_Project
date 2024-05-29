using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class JobSeekerNotFoundException : Exception
    {
        public string message;
        public JobSeekerNotFoundException()
        {
            message = "Job Seeker not found";
        }

        public JobSeekerNotFoundException(string? message)
        {
            this.message = message;
        }

        public override string Message => message;  
    }
}