using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    internal class JobSeekerSkillNotFoundException : Exception
    {
        public string message;
        public JobSeekerSkillNotFoundException()
        {
            message = "JobSeekerSkill not found";
        }

        public JobSeekerSkillNotFoundException(string? message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}