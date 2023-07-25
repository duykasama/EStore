using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eStore.Models.Models;
using eStore.Repositories.Interfaces;
using DataAccess.Repositories;

namespace eStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;
        public ProductsController()
        {
            _productRepo = new ProductRepository();
        }

        // GET: Products
        public IActionResult Index()
        {
            return _productRepo.GetAll() != null ?
                        View(_productRepo.GetAll().ToList()) :
                        Problem("Entity set is null.");
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productRepo.Add(product);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message;
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepo.Update(product);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message; 
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_productRepo.GetAll() == null)
            {
                return Problem("Entity set 'SalesManagementContext.Products'  is null.");
            }
            var product = _productRepo.GetById(id);

            if (product != null)
            {
                try
                {
                    _productRepo.Delete(product);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message;
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SearchProducts([Bind("keyword")] string keyword)
        {
            var data = string.IsNullOrEmpty(keyword) ? _productRepo.GetAll() : _productRepo.Search(keyword);
            return View("Index", data);
        }

        private bool ProductExists(int id)
        {
          return (_productRepo.GetAll()?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
