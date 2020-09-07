using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{

#pragma warning disable CS0618 // Type or member is obsolete: In preview
    public class ApiFunctionInvocationFilter : IFunctionInvocationFilter
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private const string ApiVersionResponseHeader = "x-api-version";
        private const string ApiNameResponseHeader = "x-api-name";
        private const string ApiCorrelationIdResponseReader = "x-correlation-id";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMetadataResolver _metadataResolver;

        public ApiFunctionInvocationFilter(IHttpContextAccessor httpContextAccessor, IMetadataResolver metadataResolver)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _metadataResolver = metadataResolver ?? throw new ArgumentNullException(nameof(metadataResolver));
        }

#pragma warning disable CS0618 // Type or member is obsolete: in preview
        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            return Task.CompletedTask;
        }

#pragma warning disable CS0618 // Type or member is obsolete: in preview
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var metadata = _metadataResolver.Get();
                httpContext.Response.Headers.Add(ApiVersionResponseHeader, metadata.Version);
                httpContext.Response.Headers.Add(ApiNameResponseHeader, metadata.Component);
                var correlationId = Activity.Current?.RootId;
                if (!string.IsNullOrEmpty(correlationId))
                {
                    httpContext.Response.Headers.Add(ApiCorrelationIdResponseReader, correlationId);
                }
            }

            return Task.CompletedTask;
        }
    }


}
