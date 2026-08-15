using System.Collections.Generic;
using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.Users
{
    public interface IUserQueries
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
    }
}
