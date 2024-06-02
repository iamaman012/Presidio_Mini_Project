using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class NoApplicationExistException : Exception
    {
        public string message;
        public NoApplicationExistException()
        {
            message = "No Application Exist";
        }

        public NoApplicationExistException(string? message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}