using DAL.Entitiy;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class StudentService
    {
        QLSVModel model;
        public List<Sinhvien> GetAllSinhviens()
        {
            QLSVModel model = new QLSVModel();
            return model.Sinhvien.ToList();
        }

        public void InsertNew(Sinhvien s)
        {
            model = new QLSVModel();
            model.Sinhvien.Add(s);
            model.SaveChanges();
        }
        public void InsertUpdate(Sinhvien s)
        {
            model = new QLSVModel();
            model.Sinhvien.AddOrUpdate(s);
            model.SaveChanges();
        }
        public Sinhvien FindById(string studentId)
        {
            model = new QLSVModel();
            return model.Sinhvien.FirstOrDefault(p => p.MaSV == studentId);
        }
        public void DeleteStudent(string ID)
        {
            using (QLSVModel e = new QLSVModel())
            {
                //Like That
                var selectedItem = e.Sinhvien.Where(t => t.MaSV == ID).FirstOrDefault();
                e.Sinhvien.Remove(selectedItem);
                e.SaveChanges();
            }
        }
    }
}
