using DAL.Interfaces;

namespace BLL.Services;

public class ValidationService : IValidationService
{
    private readonly IArticlesEntityProvider _articlesEntityProvider;
    private readonly ICategoriesEntityProvider _categoriesEntityProvider;

    public ValidationService(
        IArticlesEntityProvider articlesEntityProvider,
        ICategoriesEntityProvider categoriesEntityProvider)
    {
        _articlesEntityProvider = articlesEntityProvider;
        _categoriesEntityProvider = categoriesEntityProvider;
    }
    
    public async Task<bool> IsArticleTitleExistInCategory(Guid categoryId, string title)
    {
        return await _articlesEntityProvider.IsTitleExistInCategory(categoryId, title);
    }

    public async Task<bool> IsArticleTitleExistInCategory(Guid id, Guid categoryId, string title)
    {
        return await _articlesEntityProvider.IsTitleExistInCategory(id, categoryId, title);
    }

    public async Task<bool> IsCategoryWithNameExist(string name)
    {
        return await _categoriesEntityProvider.IsCategoryWithNameExist(name);
    }

    public async Task<bool> IsCategoryWithNameExist(Guid id, string name)
    {
        return await _categoriesEntityProvider.IsCategoryWithNameExist(id, name);
    }
}