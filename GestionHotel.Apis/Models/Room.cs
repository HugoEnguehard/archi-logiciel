using System;
using System.Collections.Generic;

namespace GestionHotel.Apis.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public int? Rate { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }
}
