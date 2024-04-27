using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IUnitOfWork _u;
		private readonly IMapper _mapper;

		public BasketController(IUnitOfWork u, IMapper mapper)
		{
			_u = u;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetBasketById(string Id)
		{
			var basket = await _u.BasketRepository.GetBasketAsync(Id);
			return Ok(basket ?? new CustomerBasket(Id));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateBasket(CustomerBasketDto customerBasket)
		{
			var result = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasket);
			var basket = await _u.BasketRepository.UpdateBasketAsync(result);
			return Ok(basket);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteBasket(string basketId)
		{
			return Ok(await _u.BasketRepository.DeleteBasketAsync(basketId));
		}
	}
}
