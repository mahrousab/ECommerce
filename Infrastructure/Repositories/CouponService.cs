﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	public class CouponService : ICouponService
	{
		//private readonly IConfiguration config;

		public CouponService(IConfiguration config)
        {
			//this.config = config;

			StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
		}
        public async Task<AppCoupon?> GetCouponFromPromoCode(string code)
		{
			var promotionService = new PromotionCodeService();

			var options = new PromotionCodeListOptions
			{
				Code = code
			};

			var promotionCodes = await promotionService.ListAsync(options);

			var promotionCode = promotionCodes.FirstOrDefault();

			if (promotionCode != null && promotionCode.Coupon != null)
			{
				return new AppCoupon
				{
					Name = promotionCode.Coupon.Name,
					AmountOff = promotionCode.Coupon.AmountOff,
					PercentOff = promotionCode.Coupon.PercentOff,
					CouponId = promotionCode.Coupon.Id,
					PromotionCode = promotionCode.Code
				};
			}

			return null;
		}
	}
}

