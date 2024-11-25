using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var getResult = _categoryService.GetAll();
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var getResult = await _categoryService.GetAsync(id);
        return getResult.Match<IActionResult>(
            category => RedirectToAction("ByCategoryId", "Articles", new { categoryId = category.Id }),
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryBaseData category)
    {
        if (ModelState.IsValid)
        {
            var createResult = await _categoryService.CreateAsync(category);
            return createResult.Match<IActionResult>(
                _ => RedirectToAction(nameof(Index)),
                failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
        }

        return View(category);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var getResult = await _categoryService.GetAsync(id);
        return getResult.Match<IActionResult>(
            View,
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CategoryData category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var updateResult = await _categoryService.UpdateAsync(category);
            return updateResult.Match<IActionResult>(
                updatedCategory => RedirectToAction(nameof(Index)),
                failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
        }

        return View(category);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var getResult = await _categoryService.GetAsync(id);
        return getResult.Match<IActionResult>(
            category => View(category),
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var deleteResult = await _categoryService.DeleteAsync(id);
        return deleteResult.Match<IActionResult>(
            _ => RedirectToAction(nameof(Index)),
            failedResult => View("Error", new ErrorViewModel { Message = failedResult.Message }));
    }
}