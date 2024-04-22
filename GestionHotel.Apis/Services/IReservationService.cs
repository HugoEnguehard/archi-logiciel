using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    private interface IReservationService
    {
        Task<bool> AddReservation(Reservation reservation);
        Task<Room> GetReseravtionById(int id);
        Task<bool> GetReservationByDates(DateTime start_date, DateTime end_date);
        Task<Room> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
    }
}
