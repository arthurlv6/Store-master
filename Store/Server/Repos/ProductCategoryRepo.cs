using AutoMapper;
using Store.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Server.Repos
{
    public class ProductCategoryRepo:BaseRepo
    {
        public ProductCategoryRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
    }
}
