using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Store.Shared
{
    public class PaginationModel
    {
        public int Page { get; set; } = 1;
        public int QuantityPerPage { get; set; } = 10;
    }
    public class PageDataModel<T>
    {
        public List<T> Data { get; set; }
        public int PageQuantity { get; set; }
    }
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
    }
    public class UserGetRefreshTokenModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }

    }
    public class UserToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UserRefreshTokenModel: BaseModel
    {
        [Required]
        public string RefreshToken { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
    }
    public class ProductModel : BaseModel
    {
        public string Code { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Quatity { get; set; }
        public string Description { get; set; }
        public decimal? RRP { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductLinkModel> ProductLinks { get; set; }
        public virtual ProductCategoryModel ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        //
        public bool IsShowDetail { get; set; }
    }
    public class ProductCategoryModel : BaseModel
    {
        public int ShowOrder { get; set; }
        public int? ParentId { get; set; }
    }
    public class ProductLinkModel : BaseModel
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UploadProductLinkModel
    {
        public int ProductId { get; set; }
        public IFormFile File { get; set; }
        public Stream Image { get; set; }
        public string ImageName { get; set; }
    }
    public class UserOrderModel : BaseModel
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserOrderLineModel> UserOrderLines { get; set; }
    }
    public class UserOrderLineModel : BaseModel
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Quatity { get; set; }
        public string Description { get; set; }
        public decimal? RRP { get; set; }
    }
    public class PatchUpdate
    {
        public string op { get; set; }
        public string path { get; set; }
        public string value { get; set; }
    }
    public enum PatchUpdateItem
    {
        Name,
        Style,
        Color,
        Size,
        Price,
        Quatity,
        Description
    }
    public enum SearchItem
    {
        Name,
        Category
    }
    
}
