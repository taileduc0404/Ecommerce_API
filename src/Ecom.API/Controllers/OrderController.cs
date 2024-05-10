using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrderController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto dto)
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var address = _mapper.Map<AddressDto, ShipAddress>(dto.shipToAddress);
			var order = await _orderService.CreateOrderAsync(email, dto.deliveryMethodId, dto.basketId, address);
			if (order is null)
			{
				return BadRequest(new BaseCommonResponse(400, "Cannot create order"));
			}
			return Ok(order);
		}
	}
}
