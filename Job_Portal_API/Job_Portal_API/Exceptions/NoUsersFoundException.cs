using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class NoUsersFoundException : Exception
    {   public string message;
        public NoUsersFoundException()
        {
            message = "No users found in the database";
        }

        public NoUsersFoundException(string? message)
        {
            this.message = message; 
        }
        public override string Message => message;


    }
}