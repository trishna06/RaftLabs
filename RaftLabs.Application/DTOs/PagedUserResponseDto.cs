using System.Text.Json.Serialization;

namespace RaftLabs.Application.DTOs
{
    /// <summary>
    /// DTO for Paged User list of response
    /// </summary>
    public class PagedUserResponseDto
    {
        /// <summary>
        /// Page number
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// total records
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }

        /// <summary>
        /// list of users
        /// </summary>
        [JsonPropertyName("data")]
        public List<UserDto> Data { get; set; }
    }
}
