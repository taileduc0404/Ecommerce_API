using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly IUnitOfWork _u;

		public CategoriesController(IUnitOfWork u)
		{
			_u = u;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCategory()
		{
			var allCategory = await _u.CategoryRepository.GetAllAsync();
			if (allCategory == null)
			{
				return BadRequest("Category Not Found.");
			}
			return Ok(allCategory);
		}

		[HttpGet]
		public async Task<IActionResult> GetCategoryById(int id)
		{
			var category = await _u.CategoryRepository.GetAsync(id);
			if (category == null)
			{
				return BadRequest("Category [{id}] Not Found.");
			}
			return Ok(category);
		}
	}
}
