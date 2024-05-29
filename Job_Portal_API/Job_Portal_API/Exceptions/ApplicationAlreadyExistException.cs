using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class ApplicationAlreadyExistException : Exception
    {
        public string message;
        public ApplicationAlreadyExistException()
        {
            message = "Application already exist";
        }

        public ApplicationAlreadyExistException(string? message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}