using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Helper;
using Store.Api.Repos;
using Store.Shared;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserOrderModel userOrderModel)
        {
            userOrderModel.Name = User.Identity.Name;
            userOrderModel.CreatedDate = DateTime.UtcNow;

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
        
    }
}
