using GestionHotel.Apis.Endpoints.Booking;
using GestionHotel.Apis.Enumerations;
using GestionHotel.Apis.Models;
using GestionHotel.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static Task<string> ClientArrivalDepart(HttpContext context, SampleInjectionInterface sampleInjectionInterface, [FromBody] ClientArriveBodyParams bodyParams, int userId, bool isDepart)
    {
        return sampleInjectionInterface.ClientArrivalDepartController(bodyParams, userId, isDepart);
    }
    public static Task<string> MarkRoomAsCleaned(SampleInjectionInterface sampleInjectionInterface, [FromBody] RoomBodyParams bodyParams, int userId)
    {
        return sampleInjectionInterface.MarkRoomAsCleanedController(bodyParams, userId);
    }

    public static Task<List<Room>> GetAllAvailableRooms(HttpContext context, SampleInjectionInterface sampleInjectionInterface, string start_date, string end_date)
    {
        return sampleInjectionInterface.GetAllAvailableRoomsController(start_date, end_date);
    }

    public static Task<string> ReservationRoomByDates(HttpContext context, SampleInjectionInterface sampleInjectionInterface, int userId, int roomId, string start_date, string end_date, int card_code)
    {
        return sampleInjectionInterface.ReservationRoomByDatesController(userId, roomId, start_date, end_date, card_code);
    }

    public static Task<string> DeleteReservation(HttpContext context, SampleInjectionInterface sampleInjectionInterface, int reservationId, bool refoudByReceptionist)
    {
        return sampleInjectionInterface.DeleteReservationController(reservationId, refoudByReceptionist);
    }
}

public interface SampleInjectionInterface
{
    Task<List<Room>> GetAllAvailableRoomsController(string start_date, string end_date);

    Task<string> ReservationRoomByDatesController(int userId, int roomId, string start_date, string end_date, int card_code);

    Task<string> DeleteReservationController(int reservationId, bool refoudByReceptionist);

    Task<string> ClientArrivalDepartController(ClientArriveBodyParams bodyParams, int userId, bool isDepart);

    Task<string> MarkRoomAsCleanedController(RoomBodyParams bodyParams, int userId);
}

public class SampleInjectionImplementation : SampleInjectionInterface
{
    public ApiContext _context;
    public RoomService _roomService;
    public UserService _userService;
    public ReservationService _reservationService;
    public PaiementService _paiementService;

    public SampleInjectionImplementation(ApiContext context)
    {
        _context = context;
        _roomService = new RoomService(_context);
        _userService = new UserService(_context);
        _reservationService = new ReservationService(_context);
        _paiementService = new PaiementService();
    }

    public async Task<string> ClientArrivalDepartController(ClientArriveBodyParams bodyParams, int userId, bool isDepart)
    {
        try
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId), "Missing url param : userId");
            }

            // We get user from id & check if he is a receptionnist
            User user = await _userService.GetUserById(userId);

            if (user == null || user.Type != UserType.Receptionist.ToString())
            {
                throw new Exception("Unauthorized");
            }

            // We get the room & check if it exists
            Room room = await _roomService.GetRoomById(bodyParams.roomId);

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            // We check if the action is a depart or not & update the room
            if (isDepart)
            {
                room.Occupied = RoomOccupied.NotOccupied.ToString();
                room.Cleaned = RoomCleaned.NotCleaned.ToString();
            }
            else
            {
                room.Occupied = RoomOccupied.Occupied.ToString();
            }

            // We get the current reservation for this room & check if it had been paid
            Reservation currentReservation = await _reservationService.GetCurrentReservationFromRoomId(room.Id);

            if (currentReservation == null)
            {
                throw new Exception("Cette chambre ne possède pas de réservation en cour");
            }

            if (await _roomService.UpdateRoom(room))
            {
                return $"La chambre a été mise à jour. {(currentReservation.Paid == ReservationPaid.Paid.ToString() ? "Le paiement a bien été effectué" : "Attention : Le paiement n'a pas encore eu lieu")}";
            }
            else
            {
                return "Une erreur a eu lieu lors de la mise à jour de la chambre";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<List<Room>> GetAllAvailableRoomsController(string start_date, string end_date)
    {
        var real_start_date = ParseDate(start_date);
        var real_end_date = ParseDate(end_date);

        if(real_start_date >= real_end_date){
            throw new Exception("La date de début doit être inférieur à la date de fin");
        }

        var conv_real_start_date_to_dateTime = new DateTime(real_start_date.Year, real_start_date.Month, real_start_date.Day);
        var conv_real_end_date_to_dateTime = new DateTime(real_end_date.Year, real_end_date.Month, real_end_date.Day);

        List<Room> roomsAvailable = await _roomService.GetDisponibleRoomsByDates(conv_real_start_date_to_dateTime, conv_real_end_date_to_dateTime);
        if(roomsAvailable == null || roomsAvailable.Count == 0){
            throw new Exception("Aucune chambre de disponible");
        }

        return roomsAvailable;
    }

    public async Task<string> ReservationRoomByDatesController(int userId, int roomId, string start_date, string end_date, int card_code)
    {
        ReservationPaid paid;
        var real_start_date = ParseDate(start_date);
        var real_end_date = ParseDate(end_date);

        if (real_start_date >= real_end_date)
        {
            throw new Exception("La date de début doit être inférieur à la date de fin");
        }

        if (real_start_date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new Exception("La réservation doit au moins commencer aujourd'hui");
        }

        var conv_real_start_date_to_dateTime = new DateTime(real_start_date.Year, real_start_date.Month, real_start_date.Day);
        var conv_real_end_date_to_dateTime = new DateTime(real_end_date.Year, real_end_date.Month, real_end_date.Day);

        TimeSpan difference = conv_real_end_date_to_dateTime - conv_real_start_date_to_dateTime;
        int numberOfDays = (int)Math.Floor(difference.TotalDays) + 1;

        List<Reservation> reservations = await _reservationService.GetReservationsByDates(conv_real_start_date_to_dateTime, conv_real_end_date_to_dateTime);

        if (reservations.Any(r => r.RoomId == roomId))
        {
            throw new Exception("La chambre est déjà réservée pour les dates spécifiées");
        }

        Room room = await _roomService.GetRoomById(roomId);
        if (room == null)
        {
            throw new Exception("La chambre spécifiée n'existe pas.");
        }

        var roomType = room.Type;

        if (roomType == null)
        {
            throw new Exception("Une erreur s'est produite");
        }

        int pricePerDay = (int)Enum.Parse(typeof(RoomTypePrice), roomType);

        int totalPrice = pricePerDay * numberOfDays;

        //Si la carte est pas remplie, la valeur par défaut sera 0
        if (card_code == 0)
        {
            paid = ReservationPaid.NotPaid;
        }
        else
        {
            bool hasPaid = await _paiementService.ToPay(card_code);
            if (hasPaid == true)
            {
                paid = ReservationPaid.Paid;
            }
            else
            {
                throw new Exception("Une erreur s'est produite lors du paiement");
            }
        }

        Reservation newReservation = new Reservation
        {
            UserId = userId,
            RoomId = roomId,
            StartDate = real_start_date,
            EndDate = real_end_date,
            TotalPrice = totalPrice,
            Paid = paid.ToString()
        };

        bool response = await _reservationService.AddReservation(newReservation);

        if (response == true)
        {
            return "La réservation s'est effectuée avec succès !";
        }
        else
        {
            throw new Exception("Une erreur s'est produite lors de la réservation");
        }
    }

    public async Task<string> DeleteReservationController(int reservationId, bool refoudByReceptionist)
    {
        Reservation reservation = await _reservationService.GetReservationById(reservationId);
        int? clientId = reservation.UserId;
        bool refound = false;

        if(reservation == null){
            return "La réservation n'existe pas";
        }

        DateTime currentDate = DateTime.Now;
        DateOnly reservationStartDate = (DateOnly)reservation.StartDate;
        var conv_real_start_date_to_dateTime = new DateTime(reservationStartDate.Year, reservationStartDate.Month, reservationStartDate.Day);
        TimeSpan timeUntilStart = conv_real_start_date_to_dateTime - currentDate;

        if(timeUntilStart.TotalHours <= 48){
            if(refoudByReceptionist == true){
                if(reservation.Paid == ReservationPaid.Paid.ToString())
                {
                    refound = await _paiementService.ToRefund(clientId.Value);
                }
                else
                {
                    refound = false;
                }
            }
            else
            {
                refound = false;
            }
        }
        else
        {
            if(clientId.HasValue)
            {
                refound = await _paiementService.ToRefund(clientId.Value);
            }
        }

        bool response = await _reservationService.DeleteReservation(reservationId);
        if(response == true){
            if (refound == true)
            {
                return "La réservation s'est supprimé correctement et vous avez été remboursé";
            }
            else
            {
                return "La réservation s'est supprimé correctement et n'a pas été remboursé";
            }
            
        }
        else
        {
            return "Une erreur s'est produite lors de la suppression de la réservation";
        }
    }

    public async Task<string> MarkRoomAsCleanedController(RoomBodyParams bodyParams, int userId)
    {
        try
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId), "Missing url param : userId");
            }

            // We get user from id & check if he is a receptionnist
            User user = await _userService.GetUserById(userId);

            if (user == null || user.Type != UserType.Cleaner.ToString())
            {
                throw new Exception("Unauthorized");
            }

            // We get the room & check if it exists + if it's already cleaned
            Room room = await _roomService.GetRoomById(bodyParams.roomId);

            if (room == null)
            {
                if (room.Cleaned == "Cleaned")
                {
                    throw new Exception("Room already cleaned");
                }
                throw new Exception("Room not found");
            }

            //Then we update the target room "Cleaned" status
            room.Cleaned = "Cleaned";
            await _context.SaveChangesAsync();
            return $"La chambre {bodyParams.roomId} a bien été notée comme nettoyée";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    // Méthode utilitaire pour convertir une date en objet DateOnly
    private DateOnly ParseDate(string date)
    {
        var split_date = date.Split("/");
        var day = int.Parse(split_date[0]);
        var month = int.Parse(split_date[1]);
        var year = int.Parse(split_date[2]);
        return new DateOnly(year, month, day);
        
    }
}