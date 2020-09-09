namespace Infrastructure
{
    public class MetadataResolver<T> : IMetadataResolver
    {
        private readonly ApiMetadata _metadata;

        public MetadataResolver()
        {
            var assembly = typeof(T).Assembly;
            var asmName = assembly.GetName();
            _metadata = new ApiMetadata(asmName.Name, asmName.Version.ToString());
        }

        public ApiMetadata GetMetadata() => _metadata;
    }
}
