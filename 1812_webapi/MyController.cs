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
        private readonly ILogger logger;

        public MyController(ProductDbContext context, ILogger<MyController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("all")]
        [HttpGet]
        public async Task<IResult> GetAll()
        {
            logger.LogInformation("200 OK - Sending all products");
            return TypedResults.Ok(await context.Products.ToArrayAsync());
        }

        [HttpGet("{product_id:int}")]
        public async Task<IResult> GetSingle(int product_id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product is null)
            {
                logger.LogError($"404 NOT FOUND: Product with id {product_id} was not found!");
                return TypedResults.NotFound($"Error: product was not found!");
            }
            else
            {
                logger.LogInformation($"200 OK - Sending product with id {product_id}");
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

            logger.LogInformation($"200 OK - Found {products.Count} matches to query");

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

            logger.LogInformation($"200 OK - Sending entries from {start} to {end}");

            return TypedResults.Ok(products);
        }

        [HttpPost]
        public async Task<IResult> Add([FromBody]Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            logger.LogInformation($"200 OK - New product added with id {product.Id}");
            return TypedResults.Ok(product);
        }

        [HttpPut]
        public async Task<IResult> Update([FromBody]Product product) 
        {
            if(!await context.Products.Where(p => p.Id == product.Id).AnyAsync())
            {
                logger.LogError($"404 NOT FOUND - Product with id {product.Id} was not found!");
                return TypedResults.NotFound("Product not found!");
            }

            context.Products.Update(product);
            await context.SaveChangesAsync();
            logger.LogInformation($"200 OK - Updated product with id {product.Id}");
            return TypedResults.Ok(product);
        }

        [HttpDelete]
        public async Task<IResult> Delete(int product_id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == product_id);
            if (product is null)
            {
                logger.LogError($"404 NOT FOUND - Product with id {product_id} was not found!");
                return TypedResults.NotFound($"Error: product was not found!");
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            logger.LogInformation($"200 OK - Deleted product with id {product_id}");
            return TypedResults.Ok();
        }
    }
}
