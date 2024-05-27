using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class UnauthorizedUserException : Exception
    {
        public string message;
        public UnauthorizedUserException()
        {
        }

        public UnauthorizedUserException(string? message) 
        {
            this.message = message;
        }

        public override string Message => message;
    }
}