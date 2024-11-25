using BLL.Models;
using LanguageExt.Common;

namespace BLL.Services;

public interface ICategoryService
{
    Result<List<CategoryData>> GetAll();
    
    Task<Result<CategoryData>> GetAsync(Guid id);
    
    Task<Result<CategoryData>> CreateAsync(CategoryBaseData category);
    
    Task<Result<CategoryData>> UpdateAsync(CategoryData category);
    
    Task<Result<bool>> DeleteAsync(Guid id);
}