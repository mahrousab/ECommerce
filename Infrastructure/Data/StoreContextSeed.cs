using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class StoreContextSeed
	{
		public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
		{

			if (!userManager.Users.Any(x => x.UserName == "admin@test.com"))
			{
				var user = new AppUser
				{
					UserName = "admin@test.com",
					Email = "admin@test.com",
				};

				await userManager.CreateAsync(user, "Pa$$w0rd");
				await userManager.AddToRoleAsync(user, "Admin");
			}

				if (!context.Products.Any())
			{
				var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

				var products = JsonSerializer.Deserialize<List<Product>>(productData);

				if (products == null) return;

				context.Products.AddRange(products);

				await context.SaveChangesAsync();
				

			}
			if (!context.DeliveryMethods.Any())
			{
				var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

				var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

				if (methods == null) return;

				context.DeliveryMethods.AddRange(methods);

				await context.SaveChangesAsync();


			}
		}
	}
}
