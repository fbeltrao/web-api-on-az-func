namespace Infrastructure
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class ApiFunctionTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ApiMetadata _metadata;

        public ApiFunctionTelemetryInitializer(IMetadataResolver metadataResolver)
        {
            if (metadataResolver is null)
            {
                throw new System.ArgumentNullException(nameof(metadataResolver));
            }

            _metadata = metadataResolver.GetMetadata();
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry != null)
            {
                telemetry.Context.Component.Version = _metadata.Version;
                telemetry.Context.Cloud.RoleName = _metadata.Component;
            }
        }
    }
}
