using DAL.Models;
using LanguageExt.Common;

namespace DAL.Interfaces;

public interface IArticlesEntityProvider
{
    List<Article> GetAllByCategoryId(Guid categoryId);
    
    Task<Result<Article>> GetAsync(Guid id);
    
    Task<Article> CreateAsync(Article article);
    
    Task<Result<Article>> UpdateAsync(Article article);
    
    Task DeleteAsync(Guid id);

    Task<bool> IsTitleExistInCategory(Guid categoryId, string title);
    
    Task<bool> IsTitleExistInCategory(Guid id, Guid categoryId, string title);
}