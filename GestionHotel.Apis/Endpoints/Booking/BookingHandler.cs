using GestionHotel.Apis.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Endpoints.Booking;

public static class BookingHandler
{
    public static Task<BookingView> GetAvailableRooms(HttpContext context, SampleInjectionInterface sampleInjectionInterface, [AsParameters] GetAvailableRoomsInput input)
    {
        sampleInjectionInterface.DoSomething();
        return Task.FromResult(new BookingView());
    }

    public static Task<BookingResult> Create(HttpContext context, [FromBody]BookingInput input)
    {
        return Task.FromResult(new BookingResult());
    }
}

public interface SampleInjectionInterface
{
    void DoSomething();
}

public class SampleInjectionImplementation : SampleInjectionInterface
{
    public ApiContext _context;
    public SampleInjectionImplementation (ApiContext context)
    {
        _context = context;
    }
    public void DoSomething()
    {
        var room = _context.Rooms
            .FirstOrDefault(r => r.Type == "Single");

        if (room != null) Console.WriteLine("Room found : " + room.Name);
        else Console.WriteLine("Aucune chambre de type Single n'a été trouvée");
    }
}