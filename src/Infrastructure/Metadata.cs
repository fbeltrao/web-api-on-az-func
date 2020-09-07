namespace Infrastructure
{
    public class Metadata
    {
        public Metadata(string component, string version)
        {
            Component = component;
            Version = version;
        }

        public string Version { get; }
        public string Component { get; }
    }


}
