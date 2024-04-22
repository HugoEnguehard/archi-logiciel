using GestionHotel.Apis.Enumerations;
using GestionHotel.Apis.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Apis.Services;

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
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteRoom(int id)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Room>?> GetRoomsToClean()
    {
        try
        {
            return await _context.Rooms.Where(r => r.Cleaned == RoomCleaned.NotCleaned.ToString()).ToListAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateRoom(Room room)
    {
        try
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Room?> GetRoomById(int id)
    {
        try
        {
            return await _context.Rooms.FindAsync(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<List<Room>?> GetDisponibleRoomsByDates(DateTime start_date, DateTime end_date)
    {
        return null;

        throw new NotImplementedException();
    }
}
