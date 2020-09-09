namespace Infrastructure
{
    public class ApiMetadata
    {
        public ApiMetadata(string component, string version)
        {
            Component = component;
            Version = version;
        }

        public string Version { get; }

        public string Component { get; }
    }
}
