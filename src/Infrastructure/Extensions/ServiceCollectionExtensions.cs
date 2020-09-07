using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds api headers to function response:
        /// - correlation id
        /// - function version
        /// - function assembly name
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="builder"></param>
        public static IServiceCollection AddApiResponseHeaders<TStartup>(this IServiceCollection services)
        {
            services.AddSingleton<IMetadataResolver, MetadataResolver<TStartup>>();
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddSingleton<IFunctionFilter, ApiFunctionInvocationFilter>();
#pragma warning restore CS0618 // Type or member is obsolete

            return services;
        }

        public static IServiceCollection AddApiTelemetryMetadata(this IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer, ApiFunctionTelemetryInitializer>();
            return services;
        }
    }
}
