using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class ExperienceNotFoundException : Exception
    {
        public string message;
        public ExperienceNotFoundException()
        {
            message = "Experience not found in the database";
        }

        public ExperienceNotFoundException(string? message)
        {
            this.message = message;
        }
        public override string Message => message;


    }
}