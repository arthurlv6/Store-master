using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Helper;
using Store.Api.Repos;
using Store.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepo repo;
        public ProductController(ProductRepo _repo)
        {
            repo = _repo;
        }
        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 20, string keyword = "", string categoryId = "0")
        {
            if (page < 1) return BadRequest("page can't be negative.");
            if (size < 1) return BadRequest("Page size can't be negative");

            var temp = await repo.GetPageData<Product, ProductModel>(page, size, keyword, categoryId);
            HttpContext.InsertPaginationParameterInResponse(temp.Item2);
            return Ok(temp.Item1);
        }
    }
}
