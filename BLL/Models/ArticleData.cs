using DAL.Models;
using Mapster;

namespace BLL.Models;

public record ArticleData : ArticleBaseData
{
    public Guid Id { get; init; }
}