using Ecom.API.DTOs;
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

		[HttpPost]
		public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var category = new Category
					{
						Name = categoryDto.Name,
						Description = categoryDto.Description
					};

					await _u.CategoryRepository.AddAsync(category);
					return Ok(categoryDto);
				}
				else
				{
					return BadRequest(categoryDto);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
