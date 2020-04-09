using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NativoPlusStudio.FCSServices
{
    public class FCSToken : GenericHttpClientAbstraction, IFCSToken
    {
        private readonly HttpClient _client;
        private readonly FicosoOptions _ficosoOptions;
        private readonly ILogger _logger;          


    public FCSToken(
            HttpClient client,
            IOptions<FicosoOptions> ficosoOptions,
            ILogger logger
            )
            :base(client)
        {
            _ficosoOptions = ficosoOptions.Value;
            _client = client;            
            _logger = logger;
        }      

        public async Task<IFicosoTokenResponse> GetTokenAsync()
        {
            try
            {                
                List<KeyValuePair<string, string>> tokenDetails = BuildFormUrlEncodedContent();
                var response = await PostAsync<FicosoTokenResponse>(tokenDetails, _ficosoOptions.AccessTokenEndpoint);               

                if ((!response?.Status)?? false)
                {
                    _logger.Error("#GetToken HTTP POST to FCS Authentication unsuccessful. Status code: {@StatudCode}", response?.Code);
                }               

                return response;
            }

            catch (Exception ex)
            {
                _logger.Error("#GetToken:{@AccessToken} with exception of {@Exception}", ex);
                return new FicosoTokenResponse()
                {
                    Status = false,
                    Code = HttpStatusCode.ExpectationFailed.ToString(),
                    Message = $"GetTokenAsync fail with exception of {ex.Message}"
                };
            }

        }

        private List<KeyValuePair<string, string>> BuildFormUrlEncodedContent()
        {
            return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", _ficosoOptions.GrantType),
                    new KeyValuePair<string, string>("username", _ficosoOptions.UserName),
                    new KeyValuePair<string, string>("password", _ficosoOptions.Password),
                    new KeyValuePair<string, string>("client_id", _ficosoOptions.ClientId),
                    new KeyValuePair<string, string>("client_secret", _ficosoOptions.ClientSecret),
                    new KeyValuePair<string, string>("scope", _ficosoOptions.Scope)

                };
        }
    }
}
