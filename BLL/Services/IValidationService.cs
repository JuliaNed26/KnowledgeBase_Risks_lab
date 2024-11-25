namespace BLL.Services;

public interface IValidationService
{
    Task<bool> IsArticleTitleExistInCategory(Guid categoryId, string title);

    Task<bool> IsArticleTitleExistInCategory(Guid id, Guid categoryId, string title);

    Task<bool> IsCategoryWithNameExist(string name);

    Task<bool> IsCategoryWithNameExist(Guid id, string name);
}