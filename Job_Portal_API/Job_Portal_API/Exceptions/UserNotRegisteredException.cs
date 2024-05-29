using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class UserNotRegisteredException : Exception
    {
        public string message;
        public UserNotRegisteredException()
        {
            message = "User not Registered in the database";
        }

        public override string Message => message;
    }
}