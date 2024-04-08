using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _u;

        public BasketController(IUnitOfWork u)
        {
            _u = u;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketById(string Id)
        {
            var basket = await _u.BasketRepository.GetBasketAsync(Id);
            return Ok(basket ?? new CustomerBasket(Id));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBasket(CustomerBasket customerBasket)
        {
            var basket = await _u.BasketRepository.UpdateBasketAsync(customerBasket);
            return Ok(basket);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string basketId)
        {
            return Ok(await _u.BasketRepository.DeleteBasketAsync(basketId));
        }
    }
}
