using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    public class UserNotFoundException : Exception
    {

        public string message;
        public UserNotFoundException()
        {
            message = "User not found in the database";
        }

        public UserNotFoundException(string? message) 
        {
            this.message = message;
        }
        public override string Message => message;


    }
}