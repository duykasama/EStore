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
using eStore.Repositories;

namespace eStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMemberRepository _memberRepo;
        public OrdersController()
        {
            _orderRepo =  new OrderRepository();
            _memberRepo = new MemberRepository();
        }

        // GET: Orders
        public IActionResult Index()
        {
            var salesManagementContext = _orderRepo.GetAll();
            return View(salesManagementContext.ToList());
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _orderRepo.GetAll() == null)
            {
                return NotFound();
            }

            var order = _orderRepo.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_memberRepo.GetAll(), "MemberId", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            try
            {
                _orderRepo.Add(order);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                ViewData["MemberId"] = new SelectList(_memberRepo.GetAll(), "MemberId", "Email", order.MemberId);
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _orderRepo.GetAll() == null)
            {
                return NotFound();
            }

            var order = _orderRepo.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_memberRepo.GetAll(), "MemberId", "Email", order.MemberId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            try
            {
                _orderRepo.Update(order);
            }
            catch (Exception ex)
            {
                ViewData["errorMsg"] = ex.Message;
                ViewData["MemberId"] = new SelectList(_memberRepo.GetAll(), "MemberId", "Email", order.MemberId);
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _orderRepo.GetAll() == null)
            {
                return NotFound();
            }

            var order = _orderRepo.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_orderRepo.GetAll() == null)
            {
                return Problem("Entity set 'SalesManagementContext.Orders'  is null.");
            }
            var order = _orderRepo.GetById(id);
            if (order != null)
            {
                _orderRepo.Delete(order);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SearchOrders(string keyword)
        {
            var data = string.IsNullOrEmpty(keyword) ? _orderRepo.GetAll() : _orderRepo.Search(keyword);
            return View(nameof(Index), data);
        }

        private bool OrderExists(int id)
        {
          return (_orderRepo.GetAll()?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
