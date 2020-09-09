namespace Infrastructure
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds api headers to function response:
        /// - correlation id
        /// - function version
        /// - function assembly name.
        /// </summary>
        /// <typeparam name="TStartup">Type where the main assembly is located.</typeparam>
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
