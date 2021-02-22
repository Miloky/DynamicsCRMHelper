using System.Threading.Tasks;

namespace DynamicsHelper.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateCreateAuthorizationHeader();
    }
}
