using System;
using System.Collections.Generic;

namespace GestionHotel.Apis.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Rate { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? State { get; set; }

    public int? NbPeople { get; set; }

    public string? Cleaned { get; set; }
}
