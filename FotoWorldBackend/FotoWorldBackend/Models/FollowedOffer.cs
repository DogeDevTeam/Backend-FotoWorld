using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class FollowedOffer
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public int UserId { get; set; }

    public virtual Offer Offer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
