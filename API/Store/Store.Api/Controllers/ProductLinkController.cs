﻿using Aliyun.OSS;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Repos;
using Store.Shared;
using System;
using System.IO;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLinkController : ControllerBase
    {
        private readonly ProductLinkRepo repo;
        public ProductLinkController(ProductLinkRepo _repo)
        {
            repo = _repo;
        }
        // GET: api/Product
        [HttpGet]
        public IActionResult Get(int page=0,int size=20,string keyword="")
        {
            return Ok(repo.GetAll<Product, ProductModel>(page,size,keyword));
        }
        [HttpPost]
        public IActionResult UploadFile([FromForm] UploadProductLinkModel model)
        {
            if (model == null)
                return BadRequest();

            if (model.ProductId == 0)
            {
                ModelState.AddModelError("ProductId", "ProductId shouldn't be zero");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = model.File;
            var endpoint = "oss-ap-southeast-2.aliyuncs.com";// "oss-cn-beijing.aliyuncs.com";
            var accessKeyId = "LTAI4FudBKLm3RvQHaT4vLCr";
            var accessKeySecret = "65rLNgVtajiHnEKqLsZNScDaGl3jMJ";
            var bucketName = "neartonztesting";// "storebucketfordotnetcore";
            var objectName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            // Create an OSSClient instance.
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            var stream=image.OpenReadStream();
            // Upload a file.
            client.PutObject(bucketName, objectName, stream);
            var productLinkModel = new ProductLinkModel();
            productLinkModel.ProductId = model.ProductId;
            productLinkModel.Name = objectName;
            productLinkModel.Type=Path.GetExtension(model.File.FileName);
            productLinkModel.Address= objectName;
            repo.AddProductLink(productLinkModel);
            return Ok("https://neartonztesting.oss-ap-southeast-2.aliyuncs.com/"+objectName);
        }
    }
}