using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApiContext _context;

        public RoomService(ApiContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRoom(Room room)
        {
            try
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public Task<bool> DeleteRoom(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetRoomByDates(DateTime start_date, DateTime end_date)
        {
            throw new NotImplementedException();
        }

        public Task<Room> GetRoomById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Room> UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }
    }
}
