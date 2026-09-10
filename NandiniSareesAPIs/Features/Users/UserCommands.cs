using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;

namespace NandiniSareesAPIs.Features.Users
{
    public class UserCommands : IUserCommands
    {
        private readonly IWriteDbContext _write;

        public UserCommands(IWriteDbContext write)
        {
            _write = write;
        }

        public async Task<int> CreateAsync(CreateUserRequest request)
        {
            // check unique email
            var set = _write.Set<User>();
            if (await set.AnyAsync(u => u.Email == request.Email))
            {
                // Email already exists - you could throw or return -1. Return -1 here.
                return -1;
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                Phone = request.Phone,
                IsActive = request.IsActive
            };

            set.Add(user);
            await _write.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
        {
            var set = _write.Set<User>();
            var user = await set.FindAsync(id);
            if (user == null) return false;

            if (request.FirstName != null) user.FirstName = request.FirstName;
            if (request.LastName != null) user.LastName = request.LastName;
            if (request.Phone != null) user.Phone = request.Phone;
            if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
            if (request.PasswordHash != null) user.PasswordHash = request.PasswordHash;

            user.UpdatedAt = System.DateTime.UtcNow;

            set.Update(user);
            await _write.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var set = _write.Set<User>();
            var user = await set.FindAsync(id);
            if (user == null) return false;
            set.Remove(user);
            await _write.SaveChangesAsync();
            return true;
        }
    }
}
