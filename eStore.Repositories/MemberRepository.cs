using eStore.Repositories.Interfaces;
using eStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eStore.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly SalesManagementContext _context;

        public MemberRepository()
        {
            _context = new SalesManagementContext();
        }

        // CRUD methods
        public void Add(Member member)
        {
            var m = GetById(member.MemberId);
            
            if (m is not null)
            {
                throw new Exception("MemberId already exists");
            }

            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void Delete(Member member)
        {
            var m = GetById(member.MemberId);

            if (m is null)
            {
                throw new Exception("Member does not exist");
            }

            _context.Members.Remove(member);
            _context.SaveChanges();
        }

        public IEnumerable<Member> GetAll() => _context.Members.AsNoTracking();

        public Member? GetById(int id) => _context.Members.AsNoTracking().FirstOrDefault(m => m.MemberId == id);

        public IEnumerable<Member> Search(string keyword) => _context
            .Members
            .AsNoTracking()
            .Where(
                m => m.Email.Contains(keyword)
                || m.CompanyName.Contains(keyword)
                || m.City.Contains(keyword)
                || m.Country.Contains(keyword)
                || m.Password.Contains(keyword)
            );

        public void Update(Member member)
        {
            var m = GetById(member.MemberId);

            if (m is null)
            {
                throw new Exception("Member does not exist");
            }

            _context.Members.Update(member);
            _context.SaveChanges();
        }
    }
}
