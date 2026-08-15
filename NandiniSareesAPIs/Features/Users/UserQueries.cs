using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;

namespace NandiniSareesAPIs.Features.Users
{
    public class UserQueries : IUserQueries
    {
        private readonly IReadDbContext _read;

        public UserQueries(IReadDbContext read)
        {
            _read = read;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var q = _read.Users.AsNoTracking();
            var list = await q.Select(u => new UserDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.IsActive, u.CreatedAt)).ToListAsync();
            return list;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var u = await _read.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return null;
            return new UserDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.IsActive, u.CreatedAt);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var u = await _read.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            if (u == null) return null;
            return new UserDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.IsActive, u.CreatedAt);
        }
    }
}
