namespace Infrastructure
{
    public class MetadataResolver<T> : IMetadataResolver
    {
        private readonly Metadata _metadata;

        public MetadataResolver()
        {
            var assembly = typeof(T).Assembly;
            var asmName = assembly.GetName();
            _metadata = new Metadata(asmName.Name, asmName.Version.ToString());
        }

        public Metadata Get() => _metadata;
    }


}
