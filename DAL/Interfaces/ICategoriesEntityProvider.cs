using DAL.Models;
using LanguageExt.Common;

namespace DAL.Interfaces;

public interface ICategoriesEntityProvider
{
    List<Category> GetAll();
    
    Task<Result<Category>> GetAsync(Guid id);
    
    Task<Category> CreateAsync(Category category);
    
    Task<Result<Category>> UpdateAsync(Category category);
    
    Task DeleteAsync(Guid id);

    Task<bool> IsCategoryWithNameExist(string name);
    
    Task<bool> IsCategoryWithNameExist(Guid id, string name);
}