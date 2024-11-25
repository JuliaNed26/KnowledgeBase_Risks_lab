using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleData>> GetAsync(Guid id)
    {
        var getResult = await _articleService.GetAsync(id);
        return getResult.Match<ActionResult<ArticleData>>(
            article => Ok(article),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpGet("by-category-id/{categoryId}")]
    public ActionResult<List<ArticleData>> GetAllByCategoryId(Guid categoryId)
    {
        var getResult = _articleService.GetAllByCategoryId(categoryId);
        return getResult.Match<ActionResult<List<ArticleData>>>(
            articles => Ok(articles),
            failedResult => BadRequest(failedResult.Message));
    }

    [HttpPost]
    public async Task<ActionResult<ArticleData>> CreateAsync(ArticleBaseData article)
    {
        var createResult = await _articleService.CreateAsync(article);
        return createResult.Match<ActionResult<ArticleData>>(
            createdArticle => Ok(createdArticle),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpPut]
    public async Task<ActionResult<ArticleData>> UpdateAsync(ArticleData article)
    {
        var updateResult = await _articleService.UpdateAsync(article);
        return updateResult.Match<ActionResult<ArticleData>>(
            updatedArticle => Ok(updatedArticle),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var deleteResult = await _articleService.DeleteAsync(id);
        return deleteResult.Match<ActionResult>(
            _ => Ok(),
            failedResult => BadRequest(failedResult.Message));
    }
}