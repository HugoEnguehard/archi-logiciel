using GestionHotel.Apis.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Apis.Services
{
    public class UserService : IUserService
    {
        private readonly ApiContext _context;
        public UserService(ApiContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }catch 
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if(user != null){
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }else{
                    return false;
                }
            }catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserById(int id)
        {
            try{
                var user = await _context.Users.FindAsync(id);
                if(user != null){
                    return user;
                }else{
                    return null;
                }
                
            }catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }catch
            {
                return false;
            }
        }
    }
}