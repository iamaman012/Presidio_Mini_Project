using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    public class UserAlreadyExistException : Exception
    {
        public string message;
        public UserAlreadyExistException()
        {
            message = "User already exist in the database";
        }

        public UserAlreadyExistException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}