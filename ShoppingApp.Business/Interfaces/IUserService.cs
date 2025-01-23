using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<ServiceMessage<User>> CreateUserAsync(User user, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<ServiceMessage> UpdateUserAsync(User user, int currentUserId, string newPassword = null);
        Task<ServiceMessage> DeleteUserAsync(int id);
        Task<User> AuthenticateAsync(string email, string password);
        Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto userDto);
        Task<ServiceMessage> PatchUserAsync(int id, UserPatchModelDto userDto);
    }
}
