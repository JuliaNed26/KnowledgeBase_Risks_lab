using BLL.Models;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;
using LanguageExt.Common;
using Mapster;

namespace BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoriesEntityProvider _categoriesEntityProvider;
    private readonly AbstractValidator<CategoryBaseData> _categoryBaseDataValidator;
    private readonly AbstractValidator<CategoryBaseData> _categoryDataValidator;

    public CategoryService(
        ICategoriesEntityProvider categoriesEntityProvider,
        AbstractValidator<CategoryBaseData> categoryBaseDataValidator,
        AbstractValidator<CategoryBaseData> categoryDataValidator)
    {
        _categoriesEntityProvider = categoriesEntityProvider;
        _categoryBaseDataValidator = categoryBaseDataValidator;
        _categoryDataValidator = categoryDataValidator;
    }
    
    public Result<List<CategoryData>> GetAll()
    {
        var categories = _categoriesEntityProvider.GetAll();
        return categories.Select(category => category.Adapt<CategoryData>()).ToList();
    }

    public async Task<Result<CategoryData>> GetAsync(Guid id)
    {
        var getCategoryResult = await _categoriesEntityProvider.GetAsync(id);
        return getCategoryResult.Match(
            category => new Result<CategoryData>(category.Adapt<CategoryData>()),
            ex => new Result<CategoryData>(ex));
    }

    public async Task<Result<CategoryData>> CreateAsync(CategoryBaseData category)
    {
        var validationResult = await _categoryBaseDataValidator.ValidateAsync(category);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            return new Result<CategoryData>(new ValidationException(errorMessage));
        }
        
        var categoryToCreate = category.Adapt<Category>();
        var createdArticle = await _categoriesEntityProvider.CreateAsync(categoryToCreate);
        return createdArticle.Adapt<CategoryData>();
    }

    public async Task<Result<CategoryData>> UpdateAsync(CategoryData category)
    {
        var validationResult = await _categoryDataValidator.ValidateAsync(category);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            return new Result<CategoryData>(new ValidationException(errorMessage));
        }
        
        var categoryToUpdate = category.Adapt<Category>();
        var updateCategoryResult = await _categoriesEntityProvider.UpdateAsync(categoryToUpdate);
        return updateCategoryResult.Match(
            updatedArticle => new Result<CategoryData>(updatedArticle.Adapt<CategoryData>()),
            ex => new Result<CategoryData>(ex));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        await _categoriesEntityProvider.DeleteAsync(id);
        return true;
    }
}