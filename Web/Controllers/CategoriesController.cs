using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryData>> GetAsync(Guid id)
    {
        var getResult = await _categoryService.GetAsync(id);
        return getResult.Match<ActionResult<CategoryData>>(
            category => Ok(category),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpGet("all")]
    public ActionResult<List<CategoryData>> GetAll()
    {
        var getResult = _categoryService.GetAll();
        return getResult.Match<ActionResult<List<CategoryData>>>(
            categories => Ok(categories),
            failedResult => BadRequest(failedResult.Message));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryData>> CreateAsync(CategoryBaseData category)
    {
        var createResult = await _categoryService.CreateAsync(category);
        return createResult.Match<ActionResult<CategoryData>>(
            createdCategory => Ok(createdCategory),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpPut]
    public async Task<ActionResult<CategoryData>> UpdateAsync(CategoryData category)
    {
        var updateResult = await _categoryService.UpdateAsync(category);
        return updateResult.Match<ActionResult<CategoryData>>(
            updatedCategory => Ok(updatedCategory),
            failedResult => BadRequest(failedResult.Message));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var deleteResult = await _categoryService.DeleteAsync(id);
        return deleteResult.Match<ActionResult>(
            _ => Ok(),
            failedResult => BadRequest(failedResult.Message));
    }
}