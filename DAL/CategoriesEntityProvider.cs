using DAL.Interfaces;
using DAL.Models;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace DAL;

public class CategoriesEntityProvider : ICategoriesEntityProvider
{
    private readonly KnowledgeBaseDbContext _dbContext;

    public CategoriesEntityProvider(KnowledgeBaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Category> GetAll()
    {
        return _dbContext.Categories.ToList();
    }

    public async Task<Result<Category>> GetAsync(Guid id)
    {
        var foundCategory = await _dbContext.Categories.FindAsync(id);
        return foundCategory is null 
            ? new Result<Category>(new EntityNotFoundException($"Category with id '{id}' was not found")) 
            : new Result<Category>(foundCategory);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _dbContext.Entry(category).State = EntityState.Added;
        await _dbContext.SaveChangesAsync();
        
        return category;
    }

    public async Task<Result<Category>> UpdateAsync(Category category)
    {
        var getResult = await GetAsync(category.Id);
        if (getResult.IsFaulted)
        {
            return getResult;
        }
        
        _dbContext.Entry(category).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.Categories.SingleAsync(x => x.Id == category.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var foundCategory = await _dbContext.Categories.FindAsync(id);
        if (foundCategory == null)
        {
            return;
        }
        _dbContext.Entry(foundCategory).State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsCategoryWithNameExist(string name)
    {
        return await _dbContext.Categories.AnyAsync(category => EF.Functions.Like(name, category.Name));
    }

    public async Task<bool> IsCategoryWithNameExist(Guid id, string name)
    {
        return await _dbContext.Categories.AnyAsync(category =>
            EF.Functions.Like(name, category.Name) && category.Id != id);
    }
}