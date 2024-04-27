using GestionHotel.Apis.Enumerations;
using GestionHotel.Apis.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Apis.Services;

public class RoomService : IRoomService
{
    private readonly ApiContext _context; 
    private readonly ReservationService _reservationService;

    public RoomService(ApiContext context)
    {
        _context = context;
        _reservationService = new ReservationService(_context);
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

    public async Task<Room?> GetRoomById(int Id)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(Id);
            return room;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<Room>?> GetDisponibleRoomsByDates(DateTime start_date, DateTime end_date)
    {
        try
        {
            // Récupérer réservations dans l'interval des dates
            List<Reservation> reservationList = await _reservationService.GetReservationsByDates(start_date, end_date);

            // Récupérer toutes les chambres dont l'id n'est pas dans les réservations
            var reservationIds = reservationList.Select(r => r.RoomId).ToList();

            List<Room> rooms = _context.Rooms
                .Where(room => !reservationIds.Contains(room.Id))
                .ToList();

            return rooms;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
