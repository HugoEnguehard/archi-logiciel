using System;
using System.Collections.Generic;

namespace GestionHotel.Apis.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Type { get; set; }
}
