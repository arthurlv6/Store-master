using AutoMapper;
using Store.Api.Data;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.Repos
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
            if (page < 1) page = 1;
            if (size < 1) size = 3;
            return dBContext.Set<T>()
                .Where(d=>d.Name.Contains(keyword))
                .OrderBy(d=>d.Id)
                .Skip((page -1)*size)
                .Take(size)
                .Select(d => d.ToModel<M>(mapper));
        }
        public virtual async Task<M> Add<T, M>(M m) where T : BaseEntity where M : BaseModel
        {
            T create = mapper.Map<T>(m);
            var addedEntity = dBContext.Set<T>().Add(create);
            await dBContext.SaveChangesAsync();
            return addedEntity.Entity.ToModel<M>(mapper);
        }
    }
}
