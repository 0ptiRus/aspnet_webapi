using _1812_webapi.Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1812_webapi
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("product")]
    public class MyController : ControllerBase
    {
        private ProductDbContext context;

        public MyController(ProductDbContext context)
        {
            this.context = context;
        }

        [HttpGet("all")]
        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return TypedResults.Ok(await context.Products.ToArrayAsync());
        }

        [HttpGet("{product_id:int}")]
        public async Task<IResult> GetSingle(int product_id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product is null)
            {
                return TypedResults.NotFound($"Error: product with id {product_id} was not found!");
            }
            else
            {
                return TypedResults.Ok(product);
            }
                
        }

        [HttpGet("search/{query}")]
        public async Task<IResult> GetMatchingToQuery(string query)
        {
            IList<Product> products = await context.Products
                .Where(p => p.Name
                    .ToLower()
                    .Contains(query.ToLower()))
                .ToListAsync();

            return TypedResults.Ok(products);
        }

        [HttpGet("{start}/{end}")]
        public async Task<IResult> GetRangeOfRows(int start, int end)
        {
            int page_size = end - start;

            IList<Product> products = await context.Products
                                  .OrderBy(p => p.Id) 
                                  .Skip(start)
                                  .Take(page_size)
                                  .ToListAsync();

            return TypedResults.Ok(products);
        }

        [HttpPost]
        public async Task<IResult> Add([FromBody]Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return TypedResults.Ok(product);
        }

        [HttpPut]
        public async Task<IResult> Update([FromBody]Product product) 
        {
            if(!await context.Products.Where(p => p.Id == product.Id).AnyAsync())
            {
                return TypedResults.NotFound("Product not found!");
            }

            context.Products.Update(product);
            await context.SaveChangesAsync();
            return TypedResults.Ok(product);
        }

        [HttpDelete]
        public async Task<IResult> Delete(int product_id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product is null)
            {
                return TypedResults.NotFound($"Error: product with id {product_id} was not found!");
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return TypedResults.Ok();
        }
    }
}
