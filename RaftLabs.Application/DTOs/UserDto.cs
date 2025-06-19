using System.Text.Json.Serialization;

namespace RaftLabs.Application.DTOs
{
    /// <summary>
    /// DTO for User
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// User unique id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// user's first name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// user's last name
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// user's email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO for user response
    /// </summary>
    public class UserResponseDto
    {
        [JsonPropertyName("data")]
        public UserDto Data { get; set; }
    }
}
