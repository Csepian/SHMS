using SHMS.DTO;
using SHMS.Model;

namespace SHMS.Repository
{
    public interface ITokenGenerate
    {
        public string GenerateToken(User user);
        public string GenarateToken(LoginDto loginDto);
    }
}
