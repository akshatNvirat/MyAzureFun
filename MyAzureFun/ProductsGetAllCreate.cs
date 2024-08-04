using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace MyAzureFun
{
    public class ProductsGetAllCreate
    {
        private readonly AppDbCtx _dbCtx;

        public ProductsGetAllCreate(AppDbCtx dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [FunctionName("ProductsGetAllCreate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "products")] HttpRequest req)
        {
            if (req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Product>(requestBody);
                _dbCtx.Products.Add(data);
                await _dbCtx.SaveChangesAsync();
                return new CreatedResult("/products", data);
            }

            var products = await _dbCtx.Products.ToListAsync();
            return new OkObjectResult(products);
        }
    }
}
