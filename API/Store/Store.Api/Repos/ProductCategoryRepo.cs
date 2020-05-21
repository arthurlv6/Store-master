using AutoMapper;
using Store.Api.Data;

namespace Store.Api.Repos
{
    public class ProductCategoryRepo:BaseRepo
    {
        public ProductCategoryRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
    }
}
