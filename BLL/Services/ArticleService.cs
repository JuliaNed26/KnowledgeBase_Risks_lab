using BLL.Models;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;
using LanguageExt.Common;
using Mapster;

namespace BLL.Services;

public class ArticleService : IArticleService
{
    private readonly IArticlesEntityProvider _articlesEntityProvider;
    private readonly AbstractValidator<ArticleBaseData> _articleBaseDataValidator;
    private readonly AbstractValidator<ArticleData> _articleDataValidator;

    public ArticleService(
        IArticlesEntityProvider articlesEntityProvider,
        AbstractValidator<ArticleBaseData> articleBaseDataValidator,
        AbstractValidator<ArticleData> articleDataValidator)
    {
        _articlesEntityProvider = articlesEntityProvider;
        _articleBaseDataValidator = articleBaseDataValidator;
        _articleDataValidator = articleDataValidator;
    }

    public Result<List<ArticleData>> GetAllByCategoryId(Guid categoryId)
    {
        var articles = _articlesEntityProvider.GetAllByCategoryId(categoryId);
        return articles.Select(article => article.Adapt<ArticleData>()).ToList();
    }

    public async Task<Result<ArticleData>> GetAsync(Guid id)
    {
        var getArticleResult = await _articlesEntityProvider.GetAsync(id);
        return getArticleResult.Match(
            article => new Result<ArticleData>(article.Adapt<ArticleData>()),
            ex => new Result<ArticleData>(ex));
    }

    public async Task<Result<ArticleData>> CreateAsync(ArticleBaseData article)
    {
        var validationResult = await _articleBaseDataValidator.ValidateAsync(article);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            return new Result<ArticleData>(new ValidationException(errorMessage));
        }
        
        var articleToCreate = article.Adapt<Article>();
        var createdArticle = await _articlesEntityProvider.CreateAsync(articleToCreate);
        return createdArticle.Adapt<ArticleData>();
    }

    public async Task<Result<ArticleData>> UpdateAsync(ArticleData article)
    {
        var validationResult = await _articleDataValidator.ValidateAsync(article);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(',', validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            return new Result<ArticleData>(new ValidationException(errorMessage));
        }
        
        var articleToUpdate = article.Adapt<Article>();
        var updateArticleResult = await _articlesEntityProvider.UpdateAsync(articleToUpdate);
        return updateArticleResult.Match(
            updatedArticle => new Result<ArticleData>(updatedArticle.Adapt<ArticleData>()),
            ex => new Result<ArticleData>(ex));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        await _articlesEntityProvider.DeleteAsync(id);
        return true;
    }
}