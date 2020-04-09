using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;


namespace NativoPlusStudio.FCSServices
{
    public static class AutoRegisterOauth
    {
        public static void ConfigurationFCSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FicosoOptions>(configuration.GetSection("FicosoOptions"));
            services.AddHttpClient<IFCSToken, FCSToken>(x => x.BaseAddress = new Uri(configuration["FicosoOptions:Url"])).AddAuth(services);

            
        }

        public static void AddAuth(this IHttpClientBuilder builder, IServiceCollection services)
        {
            var FCSToken = services.BuildServiceProvider().GetRequiredService<IFCSToken>();
            var accessToken = FCSToken.GetTokenAsync().GetAwaiter().GetResult();
            builder.ConfigureHttpClient(x => x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken));
          
        }

       

    }
}
