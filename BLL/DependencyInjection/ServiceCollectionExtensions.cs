using BLL.Models;
using BLL.Services;
using BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IArticleService, ArticleService>();
        serviceCollection.AddScoped<ICategoryService, CategoryService>();

        serviceCollection.AddScoped<IValidationService, ValidationService>();
        serviceCollection.AddScoped<AbstractValidator<ArticleBaseData>, ArticleBaseDataValidator>();
        serviceCollection.AddScoped<AbstractValidator<ArticleData>, ArticleDataValidator>();
        serviceCollection.AddScoped<AbstractValidator<CategoryBaseData>, CategoryBaseDataValidator>();
        serviceCollection.AddScoped<AbstractValidator<CategoryData>, CategoryDataValidator>();
        
        return serviceCollection;
    }
}