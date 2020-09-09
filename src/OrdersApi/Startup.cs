using System;
using Infrastructure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(OrdersApi.Startup))]

namespace OrdersApi
{
    class Startup : IWebJobsStartup
    {
        const string DefaultCustomerApiURL = "http://localhost:7071";

        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services
                .AddApiResponseHeaders<Startup>()
                .AddApiTelemetryMetadata();

            builder.Services.AddHttpClient<ICustomerService, CustomerService>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(configuration["CustomersApiURL"] ?? DefaultCustomerApiURL);
            });
        }
    }
}
