using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class JobListingNotFoundException : Exception
    {
        public string mesg;
        public JobListingNotFoundException()
        {
            mesg = "Job Listing Not Found";
        }

        public JobListingNotFoundException(string? message) 
        {
            mesg = message;
        }

        public override string Message => mesg; 
    }
}