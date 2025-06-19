namespace RaftLabs.Application.Configuration
{
    /// <summary>
    /// ApiSetting model from appsettings.json
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// External Url
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Cache Expiration interval
        /// </summary>
        public int CacheExpirationSeconds { get; set; } = 60;
    }
}
