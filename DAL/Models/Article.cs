namespace DAL.Models;

public class Article : Auditable
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public string Content { get; init; }

    public Guid CategoryId { get; init; }

    public Category Category { get; init; }
}