using GestionHotel.Apis.Enumerations;
using GestionHotel.Apis.Models;
using GestionHotel.Apis.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using GestionHotel.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static Task<bool> ClientArrive(HttpContext context, SampleInjectionInterface sampleInjectionInterface, [FromBody] ClientArriveBodyParams bodyParams, [FromRoute] ClientArriveBodyParams urlParams, )
    {
        sampleInjectionInterface.clientArrive(urlParams);
        return Task.FromResult(true);
    }

}
    public static Task<List<Room>> GetAllAvailableRooms(HttpContext context, SampleInjectionInterface sampleInjectionInterface, string start_date, string end_date)
    {
        return sampleInjectionInterface.GetAllAvailableRooms(start_date, end_date);
    }

    public static Task<string> ReservationRoomByDates(HttpContext context, SampleInjectionInterface sampleInjectionInterface, int userId, int roomId, string start_date, string end_date, int card_code)
    {
        return sampleInjectionInterface.ReservationRoomByDates(userId, roomId, start_date, end_date, card_code);
    }

    public static Task<string> DeleteReservation(HttpContext context, SampleInjectionInterface sampleInjectionInterface, int reservationId, bool refoudByReceptionist)
    {
        return sampleInjectionInterface.DeleteReservation(reservationId, refoudByReceptionist);
    }
}

public interface SampleInjectionInterface
{
    void clientArrive(ClientArriveBodyParams bodyParams, ClientArriveUrlParams urlParams);
    void DoSomething();

    Task<List<Room>> GetAllAvailableRooms(string start_date, string end_date);

    Task<string> ReservationRoomByDates(int userId, int roomId, string start_date, string end_date, int card_code);

    Task<string> DeleteReservation(int reservationId, bool refoudByReceptionist);
}

public class SampleInjectionImplementation : SampleInjectionInterface
{
    public ApiContext _context;
    public RoomService _roomService;
    public UserService _userService;
    public ReservationService _reservationService;
    public SampleInjectionImplementation(ApiContext context)
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
        _roomService = new RoomService(_context);
        _userService = new UserService(_context);
        _reservationService = new ReservationService(_context);
        _paiementService = new PaiementService();
    }

    public void clientArrive(ClientArriveBodyParams bodyParams, ClientArriveUrlParams urlParams)
    {
        var roomId = bodyParams.roomId;

        var room = _roomService.GetRoomById(roomId);
        if (room != null) Console.WriteLine("Room found : " + room.Name);
        else Console.WriteLine("Aucune chambre de type Single n'a �t� trouv�e");
    }

    public async Task<List<Room>> GetAllAvailableRooms(string start_date, string end_date)
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

    public async Task<string> ReservationRoomByDates(int userId, int roomId, string start_date, string end_date, int card_code)
    {
        ReservationPaid paid;
        var real_start_date = ParseDate(start_date);
        var real_end_date = ParseDate(end_date);

        if(real_start_date >= real_end_date){
            throw new Exception("La date de début doit être inférieur à la date de fin");
        }

        var nb_days = real_end_date.Day - real_start_date.Day + 1;

        var conv_real_start_date_to_dateTime = new DateTime(real_start_date.Year, real_start_date.Month, real_start_date.Day);
        var conv_real_end_date_to_dateTime = new DateTime(real_end_date.Year, real_end_date.Month, real_end_date.Day);

        List<Reservation> reservations = await _reservationService.GetReservationByDates(conv_real_start_date_to_dateTime, conv_real_end_date_to_dateTime);
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

        if(roomType == null){
            throw new Exception("Une erreur s'est produite");
        }

        int pricePerDay = (int)Enum.Parse(typeof(RoomTypePrice), roomType);
        int totalPrice = pricePerDay * nb_days;

        //Si la carte est pas remplie, la valeur par défaut sera 0
        if(card_code == 0){
            paid = ReservationPaid.NotPaid;
        }else{
            bool hasPaid = await _paiementService.ToPay(card_code);
            if(hasPaid == true){
                paid = ReservationPaid.Paid;
            }else{
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
            Paid = paid
        };

        bool response = await _reservationService.AddReservation(newReservation);

        if(response == true){
            return "La réservation s'est effectué avec succès !";
        }else{
            throw new Exception("Une erreur s'est produite lors de la réservation");
        }
    }

    public async Task<string> DeleteReservation(int reservationId, bool refoudByReceptionist)
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
                refound = await _paiementService.ToRefund(clientId.Value);
            }else{
                return "La réservation n'est pas remboursé";
            }
        }else{
            if(clientId.HasValue){
                refound = await _paiementService.ToRefund(clientId.Value);
            }
        }

        bool response = await _reservationService.DeleteReservation(reservationId);
        if(response == true){
            if (refound == true){
                return "La réservation s'est supprimé correctement et vous avez été remboursé";
            }else{
                return "La réservation s'est supprimé correctement et n'a pas été remboursé";
            }
            
        }else{
            return "Une erreur s'est produite lors de la suppression de la réservation";
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