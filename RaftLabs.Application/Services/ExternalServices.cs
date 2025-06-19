using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RaftLabs.Application.Configuration;
using RaftLabs.Application.DTOs;
using RaftLabs.Domain.Interfaces;
using RaftLabs.Domain.Models;
using System.Net;
using System.Text.Json;

namespace RaftLabs.Application.Services
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IMemoryCache _cache;
        private readonly int _cacheExpirationSeconds;

        public ExternalUserService(HttpClient httpClient, IOptions<ApiSettings> settings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _baseUrl = settings.Value.BaseUrl;
            _cache = cache;
            _cacheExpirationSeconds = settings.Value.CacheExpirationSeconds;
        }

        /// <summary>
        /// Get user details by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _cache.GetOrCreateAsync($"user_{userId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheExpirationSeconds);

                var response = await _httpClient.GetAsync($"{_baseUrl}/users/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new KeyNotFoundException("User not found");

                    throw new HttpRequestException("Failed to fetch user.");
                }

                var json = await response.Content.ReadAsStringAsync();
                var userObj = JsonSerializer.Deserialize<UserResponseDto>(json);
                var dto = userObj?.Data ?? throw new InvalidDataException("Malformed API response");

                return MapToUser(dto);
            });
        }

        /// <summary>
        /// Get lost of all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _cache.GetOrCreateAsync("all_users", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheExpirationSeconds);

                    var users = new List<User>();
                    int page = 1, totalPages = 0;

                    do
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/users?page={page}");
                        request.Headers.Add("x-api-key", "reqres-free-v1"); // Add API key header

                        var response = await _httpClient.SendAsync(request);

                        response.EnsureSuccessStatusCode();
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<PagedUserResponseDto>(json);

                        if (result == null)
                        {
                            throw new InvalidDataException("Malformed API response");
                        }
                        if (result?.Data != null)
                        {
                            users.AddRange(result.Data.Select(MapToUser));
                            totalPages = result.TotalPages;
                            page++;
                        }
                    } while (page <= totalPages);

                    return users;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private User MapToUser(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
        }
    }
}
