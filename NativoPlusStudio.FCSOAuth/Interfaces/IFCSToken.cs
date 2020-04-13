using System.Threading.Tasks;

namespace NativoPlusStudio.FCSOAuth
{
    public interface IFCSToken
    {
        Task<IFicosoTokenResponse> GetTokenAsync();
    }
}
