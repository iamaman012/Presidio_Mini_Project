using System.Runtime.Serialization;

namespace Job_Portal_API.Exceptions
{
    [Serializable]
    public class JobSkillNotFoundException : Exception
    {
        public string message;
        public JobSkillNotFoundException()
        {
            message = "JobSkill not found";
        }

        public JobSkillNotFoundException(string? message)
        {
            this.message = message;
        }
        public override string Message => message;


    }
}