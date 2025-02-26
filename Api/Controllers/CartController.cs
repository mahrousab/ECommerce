using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : BaseApiController
	{
		private readonly ICartService cart;

		public CartController(ICartService cart)
        {
			this.cart = cart;
		}

		[HttpGet]

		public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
		{
			var data = await cart.GetCartAsync(id);
			return Ok(data ?? new ShoppingCart { Id = id});
		}
		[HttpPost]

		public async Task<ActionResult<ShoppingCart>> CreateCart(ShoppingCart cart)
		{
			return Ok(cart);
		}

		[HttpDelete]

		public async Task<ActionResult<ShoppingCart>> DeleteCart(string id)
		{
			 await cart.DeleteCartAsync(id);
			return Ok();
		}
    }
}
