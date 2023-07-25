using eStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        IEnumerable<Product> Search(string keyword);
        void Update(Product product);
        void Delete(Product product);
        void Add(Product product);
    }
}
