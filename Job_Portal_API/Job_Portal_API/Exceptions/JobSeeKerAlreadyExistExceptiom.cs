using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class JobSeeKerAlreadyExistExceptiom : Exception
    {
        public string message;
        public JobSeeKerAlreadyExistExceptiom()
        {
            message = "jOB sEEKER Alread exist";
        }

        public JobSeeKerAlreadyExistExceptiom(string? message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}