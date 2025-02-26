using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CouponsController : BaseApiController
	{
        private readonly ICouponService _coupon;
        public CouponsController(ICouponService coupon)
        {
            _coupon = coupon;
        }


		[HttpGet("{code}")]
		public async Task<ActionResult<AppCoupon>> ValidateCoupon(string code)
		{
			var coupon = await _coupon.GetCouponFromPromoCode(code);

			if (coupon == null) return BadRequest("Invalid voucher code");

			return coupon;
		}
	}
}
