namespace DAL.Models;

public class Category
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public string? Description { get; init; }
    
    public ICollection<Article> Articles { get; set; }
}