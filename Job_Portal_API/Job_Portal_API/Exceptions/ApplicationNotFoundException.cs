using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class ApplicationNotFoundException : Exception
    {
        public string message;
        public ApplicationNotFoundException()
        {
            message = "Application not Exist";
        }

        public ApplicationNotFoundException(string? message)
        {
            this.message = message;
        }
        public override string Message => message;


    }
}