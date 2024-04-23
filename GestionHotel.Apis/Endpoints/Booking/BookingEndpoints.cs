namespace GestionHotel.Apis.Endpoints.Booking;


public static class BookingEndpoints
{
    private const string BASE_URL = "/api/v1/booking/";
    private const string BASE_ROOM_URL = "/api/room/";

    public static void MapBookingsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup(BASE_ROOM_URL)
            .WithOpenApi()
            .WithTags("Room");

        group.MapPost("arrive/", BookingHandler.ClientArrivalDepart)
            .WithName("Arrive");
        group.MapGet("", BookingHandler.GetAvailableRooms)
            .WithName("GetAvailableRooms");

        group.MapPost("/getAllAvailableRooms", BookingHandler.GetAllAvailableRooms)
            .WithName("GetAllAvailableRooms");
    }
}