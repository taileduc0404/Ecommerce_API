using AutoMapper;
using Ecom.API.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _u;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork u, IMapper mapper)
        {
            this._u = u;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var res = await _u.ProductRepository.GetAllAsync(x => x.Category);
            var result = _mapper.Map<List<ProductDto>>(res);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _u.ProductRepository.GetByIdAsync(id, x => x.Category);
            var res = _mapper.Map<ProductDto>(product);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(AddProductDto dto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var res = _mapper.Map<Product>(dto);
                    await _u.ProductRepository.AddAsync(res);
                    return Ok(dto);
                }
                else
                {
                    return BadRequest("Cannot add product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
