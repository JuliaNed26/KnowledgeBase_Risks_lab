using DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseLayerServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IArticlesEntityProvider, ArticlesEntityProvider>();
        serviceCollection.AddScoped<ICategoriesEntityProvider, CategoriesEntityProvider>();
        
        return serviceCollection;
    }
}