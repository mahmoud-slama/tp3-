using System;
using System.Collections.Generic;

namespace TP3.Models.Cinema;

public partial class Producer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Nationality { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
