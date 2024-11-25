using DAL.Interfaces;
using DAL.Models;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Utility.Exceptions;

namespace DAL;

public class ArticlesEntityProvider : IArticlesEntityProvider
{
    private readonly KnowledgeBaseDbContext _dbContext;
    
    public ArticlesEntityProvider(KnowledgeBaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Article> GetAllByCategoryId(Guid categoryId)
    {
        return _dbContext.Articles.Where(article => article.CategoryId == categoryId).ToList();
    }

    public async Task<Result<Article>> GetAsync(Guid id)
    {
        var foundArticle = await _dbContext.Articles.FindAsync(id);
        return foundArticle is null 
            ? new Result<Article>(new EntityNotFoundException($"Article with id '{id}' was not found")) 
            : foundArticle;
    }

    public async Task<Article> CreateAsync(Article article)
    {
        _dbContext.Entry(article).State = EntityState.Added;
        await _dbContext.SaveChangesAsync();
        
        return article;
    }

    public async Task<Result<Article>> UpdateAsync(Article article)
    {
        var getResult = await GetAsync(article.Id);
        if (getResult.IsFaulted)
        {
            return getResult;
        }
        
        _dbContext.Entry(article).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.Articles.SingleAsync(x => x.Id == article.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var foundArticle = await _dbContext.Articles.FindAsync(id);
        if (foundArticle == null)
        {
            return;
        }
        _dbContext.Entry(foundArticle).State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsTitleExistInCategory(Guid categoryId, string title)
    {
        return await _dbContext.Articles.AnyAsync(article =>
            article.CategoryId == categoryId && EF.Functions.Like(title, article.Title));
    }

    public async Task<bool> IsTitleExistInCategory(Guid id, Guid categoryId, string title)
    {
        return await _dbContext.Articles.AnyAsync(article =>
            article.CategoryId == categoryId && EF.Functions.Like(title, article.Title) && article.Id != id);
    }
}