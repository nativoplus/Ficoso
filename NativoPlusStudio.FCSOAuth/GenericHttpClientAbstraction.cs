using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NativoPlusStudio.FCSServices
{
    public abstract class GenericHttpClientAbstraction
    {
        private readonly HttpClient _client;
        public GenericHttpClientAbstraction(HttpClient client)
        {
            _client = client;
        }
        public async Task<TResponse> Get<TResponse>(string endpoint = "", string query = "") where TResponse : IBaseResponse, new()
        {
            var response = await _client
                .GetAsync($"{endpoint}{query}");
            var genericResponse = await ConvertHttpResponseToGenericResponseAsync<TResponse>(response);
            return genericResponse;
        }
        public async Task<TResponse> Post<TRequest, TResponse>(TRequest request, string endpoint = "") where TResponse : IBaseResponse, new()
        {
            var context = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client
                .PostAsync(endpoint, context);
            var genericResponse = await ConvertHttpResponseToGenericResponseAsync<TResponse>(response);
            return genericResponse;
        }

        public async Task<TResponse> PostAsync<TResponse>(IList<KeyValuePair<string, string>> request, string endpoint = "") where TResponse : IBaseResponse, new()
        {
            var context = new FormUrlEncodedContent(request);
            var response = await _client
                .PostAsync(endpoint, context);
            var genericResponse = await ConvertHttpResponseToGenericResponseAsync<TResponse>(response);
            return genericResponse;
        }
        public async Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request, string endpoint = "") where TResponse : IBaseResponse, new()
        {
            var context = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client
                .PutAsync(endpoint, context);
            var genericResponse = await ConvertHttpResponseToGenericResponseAsync<TResponse>(response);
            return genericResponse;
        }
        public async Task<TResponse> DeleteAsync<TResponse>(string endpoint = "", string query = "") where TResponse : IBaseResponse, new()
        {
            var response = await _client
                .DeleteAsync($"{endpoint}{query}");
            var genericResponse = await ConvertHttpResponseToGenericResponseAsync<TResponse>(response);
            return genericResponse;
        }
        private async Task<TResponse> ConvertHttpResponseToGenericResponseAsync<TResponse>(HttpResponseMessage response) where TResponse : IBaseResponse, new()
        {
            var genericResponse = new TResponse();
            if (response.IsSuccessStatusCode)
            {
                var data = string.Empty;
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var myStream = new StreamReader(stream))
                {
                    data = myStream.ReadToEnd();
                }
                genericResponse = JsonConvert.DeserializeObject<TResponse>(data);
            }
            genericResponse.Status = response.IsSuccessStatusCode;
            genericResponse.Code = response.StatusCode.ToString();
            genericResponse.Message = response.ReasonPhrase;
            return genericResponse;
        }
    }
}

