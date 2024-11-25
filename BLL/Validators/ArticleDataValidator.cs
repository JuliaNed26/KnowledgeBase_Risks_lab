using BLL.Models;
using BLL.Services;
using FluentValidation;

namespace BLL.Validators;

public class ArticleDataValidator : AbstractValidator<ArticleData>
{
    private readonly IValidationService _validationService;

    public ArticleDataValidator(IValidationService validationService)
    {
        _validationService = validationService;

        RuleFor(x => x).MustAsync((x, _) => BeUniqueTitleInCategory(x));
    }

    private async Task<bool> BeUniqueTitleInCategory(ArticleData article)
    {
        return !(await _validationService.IsArticleTitleExistInCategory(article.Id, article.CategoryId, article.Title));
    }
}