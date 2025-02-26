

using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
	public class StoreContext : IdentityDbContext<AppUser>
	{
        public StoreContext()
        {
            
        }
        public StoreContext(DbContextOptions<IdentityDbContext> options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
		public DbSet<Order> Orders { get; set; } 
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<AppCoupon> AppCoupans { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("server=DESKTOP-CM0INR2\\SQLEXPRESS01; database=NewECommerce; Integrated Security=true; TrustServerCertificate=True;");
	}
}
