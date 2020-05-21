using Newtonsoft.Json;
using Store.Api.Data;
using Store.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Store.Api
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext ctx;
        public DataSeeder(ApplicationDbContext _ctx)
        {
            ctx = _ctx;
        }
        public void Seed()
        {
            ctx.Database.EnsureCreated();
            if (!ctx.ProductCategories.Any())
            {
                ctx.ProductCategories.Add(new ProductCategory { Name = "Female fashion", ShowOrder = 1 });
                ctx.ProductCategories.Add(new ProductCategory { Name = "Male fashion", ShowOrder = 2 });
                ctx.ProductCategories.Add(new ProductCategory { Name = "Children", ShowOrder = 3 });
                ctx.SaveChanges();
            }
            if (!ctx.Products.Any())
            {
                var root = Directory.GetCurrentDirectory();
                using (StreamReader r = File.OpenText(@$"{root}\\Data\\initalData.json"))
                {
                    string json = r.ReadToEnd();
                    List<Product> list = JsonConvert.DeserializeObject<List<Product>>(json);
                    foreach (var item in list)
                    {
                        item.ProductCategoryId = 1;
                        item.ProductLinks = new List<ProductLink>()
                        {
                            new ProductLink() { Name = "link", Address = "https://images.unsplash.com/photo-1545594262-7df7e6050326?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=634&q=80" }
                        };
                    }
                    ctx.Products.AddRange(list);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
