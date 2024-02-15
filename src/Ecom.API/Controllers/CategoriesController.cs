using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork u, IMapper mapper)
        {
            this._u = u;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var allCategories = await _u.CategoryRepository.GetAllAsync();

            if (allCategories is not null)
            {
                var res = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ListingCategoryDto>>(allCategories);
                //var res = allCategories.Select(x => new ListingCategoryDto
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Description = x.Description
                //}).ToList();
                return Ok(res);

            }
            return BadRequest("Not Found");
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _u.CategoryRepository.GetAsync(id);
            if (category == null)
            {
                return BadRequest($"Category {id} Not Found.");
            }
            //var newCategoryDto = new ListingCategoryDto
            //{
            //    Id = category.Id,
            //    Name = category.Name,
            //    Description = category.Description
            //};
            return Ok(_mapper.Map<ListingCategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var category = new Category
                    //{
                    //    Name = categoryDto.Name,
                    //    Description = categoryDto.Description
                    //};
                    var res = _mapper.Map<Category>(categoryDto);
                    await _u.CategoryRepository.AddAsync(res);
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

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldCategory = await _u.CategoryRepository.GetAsync(categoryDto.Id);
                    if (oldCategory is not null)
                    {
                        // gán dữ liệu mới vào
                        //oldCategory.Name = categoryDto.Name;
                        //oldCategory.Description = categoryDto.Description;

                        _mapper.Map(categoryDto, oldCategory);
                        // cập nhật lại category
                        await _u.CategoryRepository.UpdateAsync(oldCategory, categoryDto.Id);
                        return Ok(categoryDto);
                    }

                }
                return BadRequest($"Category {categoryDto.Id} Not Found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existCategory = await _u.CategoryRepository.GetAsync(id);
                    if (existCategory is not null)
                    {
                        await _u.CategoryRepository.DeletetAsync(existCategory.Id);
                        return Ok($"Category {id} delete successfully.");
                    }
                }
                return BadRequest($"Category {id} Not Found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
