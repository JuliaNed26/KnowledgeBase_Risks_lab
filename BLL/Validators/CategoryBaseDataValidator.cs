using BLL.Models;
using BLL.Services;
using FluentValidation;

namespace BLL.Validators;

public class CategoryBaseDataValidator : AbstractValidator<CategoryBaseData>
{
    private readonly IValidationService _validationService;

    public CategoryBaseDataValidator(IValidationService validationService)
    {
        _validationService = validationService;

        RuleFor(x => x).MustAsync((x, _, _) => BeUniqueName(x.Name));
    }

    private async Task<bool> BeUniqueName(string name)
    {
        return !(await _validationService.IsCategoryWithNameExist(name));
    }
}