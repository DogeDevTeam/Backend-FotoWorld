using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class Offer
{
    public int Id { get; set; }

    public int OperatorId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<FollowedOffer> FollowedOffers { get; } = new List<FollowedOffer>();

    public virtual ICollection<OfferPhoto> OfferPhotos { get; } = new List<OfferPhoto>();

    public virtual Operator Operator { get; set; } = null!;
}
