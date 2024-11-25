namespace DAL.Models;

public class Auditable
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}