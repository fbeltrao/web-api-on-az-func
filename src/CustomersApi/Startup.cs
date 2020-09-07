using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Infrastructure;

[assembly: WebJobsStartup(typeof(CustomersApi.Startup))]

namespace CustomersApi
{
    class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services
                .AddApiResponseHeaders<Startup>()
                .AddApiTelemetryMetadata();
        }
    }
}
