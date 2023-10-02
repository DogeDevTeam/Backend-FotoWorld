using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class OfferPhoto
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public int PhotoId { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual Photo Photo { get; set; } = null!;
}
