
using AutoMapper;
using Store.Shared;
using System;

namespace Store.Api.Mappers
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryModel>().ReverseMap();
            CreateMap<ProductLink, ProductLinkModel>()
                .ReverseMap();
            CreateMap<UserRefreshToken, UserRefreshTokenModel>().ReverseMap();
            CreateMap<UserOrder, UserOrderModel>()
                .ReverseMap();
            CreateMap<UserOrderLine, UserOrderLineModel>()
                .ReverseMap();
        }
    }
}
