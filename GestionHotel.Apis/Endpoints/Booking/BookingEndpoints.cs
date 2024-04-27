namespace GestionHotel.Apis.Endpoints.Booking;


public static class BookingEndpoints
{
    //private const string BASE_URL = "/api/v1/booking/";
    private const string BASE_ROOM_URL = "/api/room/";

    public static void MapBookingsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup(BASE_ROOM_URL)
            .WithOpenApi()
            .WithTags("Room");

        group.MapPost("/reserveRoom", BookingHandler.ReservationRoomByDates)
            .WithName("NouvelleReservation");

        group.MapPut("/manageArrivalDepart", BookingHandler.ClientArrivalDepart)
            .WithName("GestionArriveDepartClients");

        group.MapGet("/getAllAvailableRooms", BookingHandler.GetAllAvailableRooms)
            .WithName("ListeChambresDisponibles");

        group.MapDelete("/deleteReservation", BookingHandler.DeleteReservation)
             .WithName("AnnulerReservation");
        group.MapPost("", BookingHandler.MarkRoomAsCleaned)
            .WithName("Clean");
    }
}