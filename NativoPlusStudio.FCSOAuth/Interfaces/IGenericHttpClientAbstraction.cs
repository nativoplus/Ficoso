using System.Collections.Generic;
using System.Threading.Tasks;

namespace NativoPlusStudio.FCSOAuth
{
    public interface IGenericHttpClientAbstraction
    {
        Task<TResponse> DeleteAsync<TResponse>(string endpoint = "", string query = "") where TResponse : IBaseResponse, new();
        Task<TResponse> GetAsync<TResponse>(string endpoint = "", string query = "") where TResponse : IBaseResponse, new();
        Task<TResponse> Post<TRequest, TResponse>(TRequest request, string endpoint = "") where TResponse : IBaseResponse, new();
        Task<TResponse> PostAsync<TResponse>(IList<KeyValuePair<string, string>> request, string endpoint = "") where TResponse : IBaseResponse, new();
        Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request, string endpoint = "") where TResponse : IBaseResponse, new();
    }
}