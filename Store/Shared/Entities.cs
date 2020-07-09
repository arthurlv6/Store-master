using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.Shared
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public T ToModel<T>(IMapper mapper) where T : BaseModel
        {
            return mapper.Map<T>(this);
        }
    }
    public class UserRefreshToken : BaseEntity
    {
        public string RefreshToken { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
    }
    public class Product : BaseEntity
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
        public ICollection<ProductLink> ProductLinks { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
    }
    public class ProductCategory : BaseEntity
    {
        public int ShowOrder { get; set; }
        public int? ParentId { get; set; }
    }
    public class ProductLink : BaseEntity
    {
        public string Type { get; set; }
        public string Address { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Product Product { get; set; }
    }
    public class UserOrder : BaseEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserOrderLine> UserOrderLines { get; set; }
    }
    public class UserOrderLine : BaseEntity
    {
        public int UserOrderId { get; set; }
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
}