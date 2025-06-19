using RaftLabs.Domain.Models;

namespace RaftLabs.Domain.Interfaces
{
    public interface IExternalUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}