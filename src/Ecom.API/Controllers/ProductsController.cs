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
        public ProductsController(IUnitOfWork u)
        {
            this._u = u;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var res = await _u.ProductRepository.GetAllAsync(x => x.Category);
            return Ok(res);
        }
    }
}
