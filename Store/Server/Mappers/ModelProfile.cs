
using AutoMapper;
using Store.Shared;
using System;

namespace Store.Server.Mappers
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryModel>().ReverseMap();
            CreateMap<ProductLink, ProductLinkModel>()
                .ReverseMap();
        }
    }
}
