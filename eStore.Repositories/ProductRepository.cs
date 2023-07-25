using eStore.Repositories.Interfaces;
using eStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eStore.Repositories;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesManagementContext _context;

        public ProductRepository()
        {
            _context = new();
        }

        // CRUD methods
        public void Add(Product product)
        {
            var p = GetById(product.ProductId);

            if (p is not null)
            {
                throw new Exception("ProductId already exists");
            }

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(Product product)
        {
            var p = GetById(product.ProductId);

            if (p is null)
            {
                throw new Exception("Member does not exist");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAll() => _context.Products.AsNoTracking();

        public Product? GetById(int id) => _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);

        public IEnumerable<Product> Search(string keyword) => _context
            .Products
            .AsNoTracking()
            .Where(
                p => p.ProductName.Contains(keyword)
                || p.Weight.Equals(keyword)
                || p.UnitPrice.Equals(keyword)
            );    

        public void Update(Product product)
        {
            var p = GetById(product.ProductId);

            if (p is null)
            {
                throw new Exception("Product does not exist");
            }

            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
