using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Services;
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
		public async Task<IActionResult> CreateOrder(OrderDto dto)
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

		[HttpGet]
		public async Task<IActionResult> GetDeliveryMethod()
		{
			return Ok(await _orderService.GetDeliveryMethodsAsync());
		}

		[HttpGet]
		public async Task<IActionResult> GetOrderById(int id)
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var order = await _orderService.GetOrderById(id, email);
			if (order is null)
			{
				return BadRequest(new BaseCommonResponse(404, "Not Found"));
			}
			var result = _mapper.Map<Order, OrderToReturnDto>(order);
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetOrderForUser()
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var order = await _orderService.GetOrderForUserAsync(email);
			var result = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(order);
			if (order is null)
			{
				return BadRequest(new BaseCommonResponse(404, "Not Found"));
			}
			return Ok(result);
		}
	}
}
