using eStore.Repositories.Interfaces;
using eStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesManagementContext _context;

        public OrderRepository()
        {
            _context = new();
        }

        // CRUD methods
        public void Add(Order order)
        {
            var o = GetById(order.OrderId);

            if (o is not null)
            {
                throw new Exception("OrderId already exists");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Delete(Order order)
        {
            var o = GetById(order.OrderId);

            if (o is null)
            {
                throw new Exception("Order does not exist");
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAll() => _context.Orders.AsNoTracking().Include(o => o.Member);

        public Order? GetById(int id) => _context.Orders.AsNoTracking().Include(o => o.Member).FirstOrDefault(o => o.OrderId == id);

        public IEnumerable<Order> Search(string keyword) => _context.Orders.AsNoTracking().Where(o => o.Member.Email.Contains(keyword)).Include(o => o.Member);

        public void Update(Order order)
        {
            var o = GetById(order.OrderId);

            if (o is null)
            {
                throw new Exception("Order does not exist");
            }

            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
