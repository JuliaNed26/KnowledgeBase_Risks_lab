using BLL.Models;
using BLL.Services;
using FluentValidation;

namespace BLL.Validators;

public class ArticleBaseDataValidator : AbstractValidator<ArticleBaseData>
{
    private readonly IValidationService _validationService;

    public ArticleBaseDataValidator(IValidationService validationService)
    {
        _validationService = validationService;

        RuleFor(x => x).MustAsync((x, _, _) => BeUniqueTitleInCategory(x.CategoryId, x.Title));
    }

    private async Task<bool> BeUniqueTitleInCategory(Guid categoryId, string title)
    {
        return !(await _validationService.IsArticleTitleExistInCategory(categoryId, title));
    }
}