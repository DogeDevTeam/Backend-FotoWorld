using System;
using System.Collections.Generic;

namespace FotoWorldBackend.Models;

public partial class OperatorRating
{
    public int Id { get; set; }

    public int OperatorId { get; set; }

    public int UserId { get; set; }

    public double Stars { get; set; }

    public string Comment { get; set; } = null!;

    public virtual Operator Operator { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
