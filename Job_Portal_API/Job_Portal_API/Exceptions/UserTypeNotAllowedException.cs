using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class UserTypeNotAllowedException : Exception
    {
        public string message;
        public UserTypeNotAllowedException()
        {
            message = "You are not Allowed to Add Employer";
        }
        public UserTypeNotAllowedException(string mesg)
        {
            message = mesg;
        }

        public override string Message => message;
    }
}