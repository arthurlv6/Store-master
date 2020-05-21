using AutoMapper;
using Store.Server.Data;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Server.Repos
{
    public class BaseRepo
    {
        protected readonly ApplicationDbContext dBContext;
        protected readonly IMapper mapper;
        public BaseRepo(ApplicationDbContext _dBContext, IMapper _mapper)
        {
            dBContext = _dBContext;
            mapper = _mapper;
        }
        public virtual IEnumerable<M> GetAll<T, M>(int page=1,int size=20,string keyword = "") where T : BaseEntity where M : BaseModel
        {
            return dBContext.Set<T>()
                .Where(d=>d.Name.Contains(keyword))
                .OrderBy(d=>d.Id)
                .Skip((page -1)*size)
                .Take(size)
                .Select(d => d.ToModel<M>(mapper));
        }
    }
}
