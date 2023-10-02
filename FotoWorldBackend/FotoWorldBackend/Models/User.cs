using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsOperator { get; set; }

    public virtual ICollection<FollowedOffer> FollowedOffers { get; } = new List<FollowedOffer>();

    public virtual ICollection<OperatorRating> OperatorRatings { get; } = new List<OperatorRating>();

    public virtual ICollection<Operator> Operators { get; } = new List<Operator>();
}
