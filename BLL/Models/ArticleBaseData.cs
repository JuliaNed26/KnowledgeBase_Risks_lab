using DAL.Models;
using Mapster;

namespace BLL.Models;

public record ArticleBaseData
{
    public string Title { get; init; }

    public string Content { get; init; }

    public Guid CategoryId { get; init; }
}