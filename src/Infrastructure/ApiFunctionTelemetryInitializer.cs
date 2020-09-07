using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Infrastructure
{
    public class ApiFunctionTelemetryInitializer : ITelemetryInitializer
    {
        private readonly Metadata _metadata;

        public ApiFunctionTelemetryInitializer(IMetadataResolver metadataResolver)
        {
            _metadata = metadataResolver.Get();
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Component.Version = _metadata.Version;
            telemetry.Context.Cloud.RoleName = _metadata.Component;
        }
    }
}
