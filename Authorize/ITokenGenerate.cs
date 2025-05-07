using SHMS.Model;

namespace SHMS.Authorize
{
    public interface ITokenGenerate
    {
    public string GenerateToken(User user);
        
    }
}
