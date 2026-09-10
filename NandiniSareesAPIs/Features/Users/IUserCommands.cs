using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.Users
{
    public interface IUserCommands
    {
        Task<int> CreateAsync(CreateUserRequest request);
        Task<bool> UpdateAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
