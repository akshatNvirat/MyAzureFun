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
    public class ProductsGetByIdUpdateDelete
    {

        private readonly AppDbCtx _dbCtx;

        public ProductsGetByIdUpdateDelete(AppDbCtx dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [FunctionName("ProductsGetByIdUpdateDelete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", Route = "products/{id}")] HttpRequest req
            , int id)
        {
            if (req.Method == HttpMethods.Get)
            {
                var data = await _dbCtx.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null) return new NotFoundResult();

                return new OkObjectResult(data);
            }

            else if (req.Method == HttpMethods.Put)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBody);
                product.Id = id;
                _dbCtx.Products.Update(product);
                await _dbCtx.SaveChangesAsync();

                return new OkObjectResult(product);
            }

            else
            {
                var product = await _dbCtx.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product == null) return new NotFoundResult();

                _dbCtx.Products.Remove(product);
                await _dbCtx.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
