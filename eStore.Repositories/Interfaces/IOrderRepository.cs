using eStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        IEnumerable<Order> Search(string keyword);
        void Update(Order order);
        void Delete(Order order);
        void Add(Order order);
    }
}
