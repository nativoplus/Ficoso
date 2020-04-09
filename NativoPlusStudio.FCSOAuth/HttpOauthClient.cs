using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NativoPlusStudio.FCSServices
{
    public abstract class HttpOAuthClient
    {

        private readonly HttpClient _client;        
        private readonly IFCSToken _tokenHelper;   



        public HttpOAuthClient(IFCSToken tokenHelper)
            
        {
            _tokenHelper = tokenHelper;            
            ConfigureHttpClient().Wait();

        }
        public async Task ConfigureHttpClient()
        {           
            var accessToken = await _tokenHelper.GetTokenAsync();            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);       
            
        }

        public HttpClient GetHttpClient()
        {
            return _client;
        }        

    }
}
