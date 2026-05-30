using APIproject.Domain.Entities.Models;

namespace APIproject.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserList?> GetByEmailAsync(string email);
        Task<UserList?> GetByIdAsync(short id);
        Task<bool> EmailExistsAsync(string email);
        Task<short> GetNextUserIdAsync();
        Task AddAsync(UserList user);
        Task UpdateAsync(UserList user);
    }
}
