using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudSigns
{
    static class StudentEditor
    {
        static private StudentContext DataBase;
        static private ObservableCollection<Student> students;

        static public void Init()
        {
            DataBase = new StudentContext();
        }

        static public ObservableCollection<Student> GetStudents()
        {
            DataBase = new StudentContext();
            DataBase.Students.Load();
            students = DataBase.Students.Local;
            return students;
        }

        static public void SaveChanges()
        {
            DataBase.SaveChanges();
        }
    }
}
