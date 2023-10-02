using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class Operator
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public bool IsCompany { get; set; }

    public string Availability { get; set; } = null!;

    public string LocationCity { get; set; } = null!;

    public int OperatingRadius { get; set; }

    public bool Photo { get; set; }

    public bool DronePhoto { get; set; }

    public bool Filming { get; set; }

    public bool DroneFilming { get; set; }

    public virtual User Account { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual ICollection<OperatorRating> OperatorRatings { get; } = new List<OperatorRating>();
}
