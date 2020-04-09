using System.Threading.Tasks;

namespace NativoPlusStudio.FCSServices
{
    public interface IFCSToken
    {
        Task<IFicosoTokenResponse> GetTokenAsync();
    }
}
