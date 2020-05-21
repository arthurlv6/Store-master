using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Repos;
using Store.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryRepo repo;
        public ProductCategoryController(ProductCategoryRepo _repo)
        {
            repo = _repo;
        }
        // GET: api/ProductCategory
        [HttpGet]
        public IActionResult Get(int page=1,int size=20,string keyword="")
        {
            var temp = repo.GetAll<ProductCategory,ProductCategoryModel>(page,size,keyword);
            return Ok(temp);
        }
    }
}
