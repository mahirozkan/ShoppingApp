using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Types;
using ShoppingApp.Data.Entities;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user, string password);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user, int currentUserId, string newPassword = null);
        Task DeleteUserAsync(int id);
        Task<User> AuthenticateAsync(string email, string password);
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
