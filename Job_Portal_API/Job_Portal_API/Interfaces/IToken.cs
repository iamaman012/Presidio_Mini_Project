using Job_Portal_API.Models;

namespace Job_Portal_API.Interfaces
{
    public interface IToken
    {
        public string GenerateJSONWebToken(User user);
    }
}
