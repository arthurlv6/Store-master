using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrdersController : ControllerBase
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateStartOrStop(int id, [FromBody] JsonPatchDocument<ProductModel> patchDoc)
        {
            if (id == 0)
                return BadRequest();

            var requirement = await repo.GetOneAsync(id);
            if (requirement == null)
                return NotFound();

            patchDoc.ApplyTo(requirement, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await repo.UpdateRequirementAsync(requirement);
            return NoContent();
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserOrdersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserOrdersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserOrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserOrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
