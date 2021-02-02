using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace StudSigns
{
    class AdminEditor
    {
        static private AdministratorContext DataBase;
        static private ObservableCollection<Administrator> admins;

        static public void Init()
        {
            DataBase = new AdministratorContext();
        }

        static public ObservableCollection<Administrator> GetAdmins()
        {
            DataBase = new AdministratorContext();
            DataBase.Admins.Load();
            admins = DataBase.Admins.Local;
            return admins;
        }

        static public void SaveChanges()
        {
            DataBase.SaveChanges();
        }
    }
}
