using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eStore.Models.Models;
using eStore.Repositories.Interfaces;
using eStore.Repositories;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepo;
        public MembersController()
        {
            _memberRepo = new MemberRepository();
        }

        // GET: Members
        public IActionResult Index()
        {
            return _memberRepo.GetAll() != null ?
                        View(_memberRepo.GetAll().ToList()) :
                        Problem("Entity set 'SalesManagementContext.Members'  is null.");
        }

        // GET: Members/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _memberRepo.GetAll() == null)
            {
                return NotFound();
            }

            var member = _memberRepo.GetById(id.Value);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _memberRepo.Add(member);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message;
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _memberRepo.GetAll() == null)
            {
                return NotFound();
            }

            var member = _memberRepo.GetById(id.Value);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _memberRepo.Update(member);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message;
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _memberRepo.GetAll() == null)
            {
                return NotFound();
            }

            var member = _memberRepo.GetById(id.Value);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_memberRepo.GetAll() == null)
            {
                return Problem("Entity set 'SalesManagementContext.Members'  is null.");
            }
            var member = _memberRepo.GetById(id);
            if (member != null)
            {
                try
                {
                    _memberRepo.Delete(member);
                }
                catch (Exception ex)
                {
                    TempData["errorMsg"] = ex.Message;
                    return Redirect(HttpContext.Request.Headers["Referer"]);
                }

                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SearchMembers([Bind("keyword")] string keyword)
        {
            var data = string.IsNullOrEmpty(keyword) ? _memberRepo.GetAll() : _memberRepo.Search(keyword);
            return View(nameof(Index), data);
        }

        private bool MemberExists(int id)
        {
            return _memberRepo.GetById(id) != null;
        }
    }
}
