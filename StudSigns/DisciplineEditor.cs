using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace StudSigns
{
    static class DisciplineEditor
    {
        static private DisciplineContext DataBase;
        static private ObservableCollection<Discipline> students;

        static public void Init()
        {
            DataBase = new DisciplineContext();
        }

        static public ObservableCollection<Discipline> GetDisciplines()
        {
            DataBase = new DisciplineContext();
            DataBase.Disciplines.Load();
            students = DataBase.Disciplines.Local;
            return students;
        }

        static public void SaveChanges()
        {
            DataBase.SaveChanges();
        }
    }
}
