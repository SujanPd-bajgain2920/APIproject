using APIproject.Domain.Entities.Models;
using APIproject.Domain.Interfaces;
using APIproject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace APIproject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FypContext _context;

        public UserRepository(FypContext context)
        {
            _context = context;
        }

        public async Task<UserList?> GetByEmailAsync(string email)
        {
            return await _context.UserLists
                .FirstOrDefaultAsync
                (x => x.EmailAddress.ToUpper() == email.ToUpper());
        }

        public async Task<UserList?> GetByIdAsync(short id)
        {
            return await _context.UserLists
                .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.UserLists
                .AnyAsync(x => x.EmailAddress == email);
        }

        public async Task<short> GetNextUserIdAsync()
        {
            return _context.UserLists.Any()
                ? (short)(_context.UserLists.Max(x => x.UserId) + 1)
                : (short)1;
        }

        public async Task AddAsync(UserList user)
        {
            _context.UserLists.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserList user)
        {
            _context.UserLists.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
