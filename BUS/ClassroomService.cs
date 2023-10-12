using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entitiy;

namespace BUS
{
    public class ClassroomService
    {
        public List<Lop> GetAllClassrooms()
        {
            QLSVModel context = new QLSVModel();
            return context.Lop.ToList();
        }
    }
}
