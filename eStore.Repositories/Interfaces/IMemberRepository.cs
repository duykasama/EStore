using eStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        IEnumerable<Member> Search(string keyword);
        void Update(Member member);
        void Delete(Member member);
        void Add(Member member);
    }
}
