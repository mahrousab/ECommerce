using Api.DTOs;
using Api.Extensions;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : BaseApiController
	{
		private readonly IUnitOfWork unit;
		private readonly IPaymentService paymentService;

		public AdminController(IUnitOfWork unit, IPaymentService paymentService)
        {
			this.unit = unit;
			this.paymentService = paymentService;
		}

		[HttpGet("orders")]
		public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders([FromQuery] OrderSpecParams specParams)
		{
			var spec = new OrderSpecification(specParams);

			return await CreatePagedResult(unit.Repository<Order>(), spec, specParams.PageIndex,
				specParams.PageSize, o => o.ToDto());
		}

		[HttpGet("orders/{id:int}")]
		public async Task<ActionResult<OrderDto>> GetOrderById(int id)
		{
			var spec = new OrderSpecification(id);

			var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

			if (order == null) return BadRequest("No order with that id");

			return order.ToDto();
		}

		[HttpPost("orders/refund/{id:int}")]
		public async Task<ActionResult<OrderDto>> RefundOrder(int id)
		{
			var spec = new OrderSpecification(id);

			var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

			if (order == null) return BadRequest("No order with that id");

			if (order.Status == OrderStatus.Pending)
				return BadRequest("Payment not received for this order");

			var result = await paymentService.RefundPayment(order.PaymentIntentId);

			if (result == "succeeded")
			{
				order.Status = OrderStatus.Refunded;

				await unit.Complete();

				return order.ToDto();
			}

			return BadRequest("Problem refunding order");
		}
	}
}
