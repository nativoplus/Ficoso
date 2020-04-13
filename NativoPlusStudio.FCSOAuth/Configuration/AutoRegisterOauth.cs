using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Polly;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NativoPlusStudio.FCSOAuth
{
    public static class AutoRegisterOauth
    {
        public static void ConfigurationFCSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FicosoOptions>(configuration.GetSection("FicosoOptions"));            
            services.AddHttpClient<IFCSToken, FCSToken>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("FicosoOptions").Get<FicosoOptions>().Url);

            })
                 .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                 {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }))
                .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                )).AddAuth(services);   

        }       

        public static void AddAuth(this IHttpClientBuilder builder, IServiceCollection services)
        {
            var FCSToken = services.BuildServiceProvider().GetRequiredService<IFCSToken>();
            var accessToken = FCSToken.GetTokenAsync().GetAwaiter().GetResult();
            builder.ConfigureHttpClient(x => x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken));
          
        }

       

    }
}
