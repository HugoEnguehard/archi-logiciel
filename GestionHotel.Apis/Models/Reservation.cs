using System;
using System.Collections.Generic;

namespace GestionHotel.Apis.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? UserId { get; set; }

    public int? RoomId { get; set; }

    public double? TotalPrice { get; set; }
}
