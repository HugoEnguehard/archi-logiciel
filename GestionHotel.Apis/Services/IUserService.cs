using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public interface IUserService
    {
        Task<bool> AddUser(User user);
        Task<User?> GetUserById(int id);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
