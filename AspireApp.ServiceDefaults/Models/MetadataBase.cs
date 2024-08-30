namespace AspireApp.ServiceDefaults.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataBase
    {
        public Guid Id { get; set; }
        public string Metadata { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public MetadataBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
