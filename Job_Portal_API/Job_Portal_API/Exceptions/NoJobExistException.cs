using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class NoJobExistException : Exception
    {   
        public string message;
        public NoJobExistException()
        {
            
        }

        public NoJobExistException(string? message)
        {
           this.message = message;
        }

       public string Message => message;
    }
}