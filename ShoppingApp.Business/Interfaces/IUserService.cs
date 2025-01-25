using ShoppingApp.Business.Dtos; // DTO'ları içeren namespace
using ShoppingApp.Business.Types; // Hizmet mesajı tanımlarını içeren namespace
using ShoppingApp.Data.Entities; // Veri varlıklarını içeren namespace
using System.Threading.Tasks; // Asenkron operasyonlar için gerekli namespace

namespace ShoppingApp.Business.Interfaces
{
    // Kullanıcı hizmetlerine ilişkin metotları tanımlayan arayüz
    public interface IUserService
    {
        Task<ServiceMessage<User>> CreateUserAsync(User user, string password); // Yeni bir kullanıcı oluşturur.

        Task<User> GetUserByIdAsync(int id); // Belirli bir ID'ye sahip kullanıcıyı getirir.

        Task<ServiceMessage> UpdateUserAsync(User user, int currentUserId, string newPassword = null); // Kullanıcıyı günceller ve isteğe bağlı olarak yeni bir şifre belirler.

        Task<ServiceMessage> DeleteUserAsync(int id); // Belirtilen ID'ye sahip kullanıcıyı siler.

        Task<User> AuthenticateAsync(string email, string password); // Kullanıcı kimlik doğrulaması yapar.

        Task<ServiceMessage<UserInfoDto>> LoginUserAsync(LoginUserDto userDto); // Kullanıcı girişi yapar ve kullanıcı bilgilerini döner.

        Task<ServiceMessage> PatchUserAsync(int id, UserPatchModelDto userDto); // Kullanıcı bilgilerini kısmi olarak günceller.
    }
}
