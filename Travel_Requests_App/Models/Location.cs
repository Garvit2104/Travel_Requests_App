using System;
using System.Collections.Generic;

namespace Travel_Requests_App.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
}
