namespace Infrastructure
{
    public interface IMetadataResolver
    {
        /// <summary>
        /// Gets the <see cref="ApiMetadata"/>.
        /// </summary>
        ApiMetadata GetMetadata();
    }
}
