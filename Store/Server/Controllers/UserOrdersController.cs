using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Store.Server.Repos;
using Store.Shared;

namespace Store.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserOrdersController : ControllerBase
    {
        private readonly UserOrderRepo repo;
        private readonly ProductRepo productRepo;
        public UserOrdersController(UserOrderRepo _repo, ProductRepo _productRepo)
        {
            repo = _repo;
            productRepo = _productRepo;
        }
        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 20, string keyword = "")
        {
            if (page < 1) return BadRequest("page can't be negative.");
            if (size < 1) return BadRequest("Page size can't be negative");

            var temp = await repo.GetPageData<UserOrder, UserOrderModel>(page, size, keyword);
            HttpContext.InsertPaginationParameterInResponse(temp.Item2);
            return Ok(temp.Item1);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateStartOrStop(int id, [FromBody] JsonPatchDocument<UserOrderModel> patchDoc)
        {
            if (id == 0)
                return BadRequest();

            var orderModel = await repo.GetOneAsync(id);
            if (orderModel == null)
                return NotFound();

            patchDoc.ApplyTo(orderModel, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await repo.UpdateRequirementAsync(orderModel);
            return NoContent();
        }
        

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserOrderModel userOrderModel)
        {
            var temp = User;
            if (userOrderModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            foreach (var item in userOrderModel.UserOrderLines)
            {
                if (!productRepo.ValidateId(item.ProductId))
                {
                    return BadRequest("Product Id was not validated.");
                }
            }
            
            var created = await repo.Add<UserOrder,UserOrderModel>(userOrderModel);

            return Created("UserOrder", created);
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
