using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public virtual ICollection<OfferPhoto> OfferPhotos { get; } = new List<OfferPhoto>();
}
