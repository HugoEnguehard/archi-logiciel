﻿using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services
{
    public interface IReservationService
    {
        Task<bool> AddReservation(Reservation reservation);
        Task<Reservation> GetReservationById(int id);
        Task<List<Reservation>> GetReservationsByDates(DateTime start_date, DateTime end_date);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
        Task<Reservation?> GetCurrentReservationFromRoomId(int roomId);
    }
}
