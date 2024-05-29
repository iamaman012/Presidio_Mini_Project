using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class InvalidJobTypeException : Exception
    {
        public string message;
        public InvalidJobTypeException()
        {
            message = "Invalid Job Type";
        }

        public InvalidJobTypeException(string? message) 
        {
            this.message= message;  
        }

       public override string Message => message;
    }
}