using BLL.Models;
using BLL.Services;
using FluentValidation;

namespace BLL.Validators;

public class CategoryDataValidator : AbstractValidator<CategoryData>
{
    private readonly IValidationService _validationService;

    public CategoryDataValidator(IValidationService validationService)
    {
        _validationService = validationService;

        RuleFor(x => x).MustAsync((x, _, _) => BeUniqueName(x.Id, x.Name));
    }

    private async Task<bool> BeUniqueName(Guid id, string name)
    {
        return !(await _validationService.IsCategoryWithNameExist(id, name));
    }
}