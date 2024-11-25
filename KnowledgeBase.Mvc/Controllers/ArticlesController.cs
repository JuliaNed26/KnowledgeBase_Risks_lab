using BLL.Models;
using BLL.Services;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ArticlesController : Controller
{
    private const string CategoryIdName = "CategoryId";
    
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;
    private IMemoryCache _memoryCache;

    public ArticlesController(
        IArticleService articleService,
        ICategoryService categoryService, IMemoryCache memoryCache)
    {
        _articleService = articleService;
        _categoryService = categoryService;
        _memoryCache = memoryCache;
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var getResult = await _articleService.GetAsync(id);
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }
    
    [HttpGet("ByCategoryId/{categoryId}")]
    public async Task<IActionResult> ByCategoryId(Guid categoryId)
    {
        var getCategoryResult = await _categoryService.GetAsync(categoryId);
        getCategoryResult.IfFail(failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
        getCategoryResult.IfSucc(category => ViewData["CategoryName"] = category.Name);
        _memoryCache.Set(CategoryIdName, categoryId);
        
        var getResult = _articleService.GetAllByCategoryId(categoryId);
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    public IActionResult Create()
    {
        var categoryId = (Guid)_memoryCache.Get(CategoryIdName)!;
        return View(new ArticleBaseData { CategoryId = categoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleBaseData article)
    {
        if (ModelState.IsValid)
        {
            var createResult = await _articleService.CreateAsync(article);
            return createResult.Match<IActionResult>(
                createdArticle => RedirectToAction(nameof(Details), new { createdArticle.Id }),
                failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
        }

        return View(article);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var getResult = await _articleService.GetAsync(id);
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ArticleData article)
    {
        if (id != article.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var updateResult = await _articleService.UpdateAsync(article);
            return updateResult.Match<IActionResult>(
                updatedArticle => RedirectToAction(nameof(Details), new { updatedArticle.Id }),
                failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
        }

        return View(article);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var getResult = await _articleService.GetAsync(id);
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, Guid categoryId)
    {
        var deleteResult = await _articleService.DeleteAsync(id);
        return deleteResult.Match<IActionResult>(
            _ => RedirectToAction(nameof(ByCategoryId), new { CategoryId = categoryId }),
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }
}