using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RaftLabs.Application.Services;
using RaftLabs.Domain.Interfaces;

namespace RaftLabs.Application
{
    public static class RaftLabsApplicationExtension
    {
        public static IServiceCollection AddRaftLabsApplication(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpClient<IExternalUserService, ExternalUserService>()
                .AddPolicyHandler(GetRetryPolicy());
            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
