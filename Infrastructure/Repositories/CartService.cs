using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace Infrastructure.Repositories
{
	public class CartService : ICartService
	{

		private readonly IDistributedCache cash;
        public CartService(IDistributedCache cash)
        {
			this.cash = cash;
        }
        public async Task DeleteCartAsync(string key)
		{
			 await cash.RemoveAsync(key);
		}

		public async Task<ShoppingCart?> GetCartAsync(string key)
		{
	     	var data =  await cash.GetStringAsync(key);

			return data.IsNullOrEmpty() ? null : JsonSerializer.Deserialize<ShoppingCart?>(data);
		}

		public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
		{
			throw new Exception("That is not implmentaion method");
		}

	}
}
