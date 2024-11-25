using BLL.Models;
using LanguageExt.Common;

namespace BLL.Services;

public interface IArticleService
{
    Result<List<ArticleData>> GetAllByCategoryId(Guid categoryId);
    
    Task<Result<ArticleData>> GetAsync(Guid id);
    
    Task<Result<ArticleData>> CreateAsync(ArticleBaseData article);
    
    Task<Result<ArticleData>> UpdateAsync(ArticleData article);
    
    Task<Result<bool>> DeleteAsync(Guid id);
}