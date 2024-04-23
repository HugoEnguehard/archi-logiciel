using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApiContext _context;

        public ReservationService(ApiContext context)
        {
            _context = context;
        }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public Task<bool> DeleteReservation(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservavtion != null)
                {
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> GetReservationByDates(DateTime start_date, DateTime end_date)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> GetReservationById(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    return reservation;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                return null;
            }
        }

        public Task<Reservation> UpdateReservation(Reservation reservation)
        {
            try
            {
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
