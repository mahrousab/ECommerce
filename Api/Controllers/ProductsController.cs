using Api.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
	
	public class ProductsController : BaseApiController
	{
		
		private readonly IUnitOfWork unit;
		public ProductsController(IUnitOfWork unit)
        {
			
			this.unit = unit;
		}

        [HttpGet]

		public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
			[FromQuery]ProductSpecParams specParams)
		{
			var spec = new ProductSpecification(specParams);

			
			return await CreatePageResult(unit.Repository <Product>(), spec,specParams.PageIndex,specParams.PageSize);
		}

		[HttpGet("{id:int}")]

		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await unit.Repository<Product>().GetByIdAsync(id);

			if(product == null)
			{
				return NotFound();
			}
			return product;
		}
		[HttpGet("brands")]

		public async Task<ActionResult <IReadOnlyList<string>>> GetBrands()
		{
			var spec = new BrandListSpecification();

			return Ok(await unit.Repository<Product>().ListAsync(spec)) ;
		}


		[HttpGet("Types")]

		public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
		{
			var spec = new TypeListSpecification();

			return Ok(await unit.Repository<Product>().ListAsync(spec));
		}


		[Authorize(Roles ="Admin")]
		[HttpPost]
		public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
		{
			unit.Repository<Product>().Add(product);


			if(await unit.Complete())
			{
				return CreatedAtAction("get product", new {id =product.Id}, product);
			}

			return BadRequest("problem");
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("{id:int}")]

		public async Task<ActionResult> UpdateProduct(int id, Product product)
		{
			if(product.Id!=id || !ProductExist(id))
			{
				return BadRequest("can not update this request");
			}
			unit.Repository<Product>().Update(product);
			if(await unit.Complete())
			{
				return NoContent();
			}
			return BadRequest("problem updating product");
			
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			var product = await unit.Repository<Product>().GetByIdAsync(id);

			if(product == null)
			{
				return NotFound();
			}
			unit.Repository<Product>().Remove(product);
			if (await unit.Complete())
			{
				return NoContent();
			}
			return BadRequest("problem delete product");
		}

		private bool ProductExist(int id)
		{
			return unit.Repository<Product>().Exists(id);
		}
    }
}
