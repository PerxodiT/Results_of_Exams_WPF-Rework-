using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudSigns
{
    static class DBInterface
    {
        static public bool StudentInput(string id)
        {
            using (StudentContext db = new StudentContext())
                return db.Students.Find(id) != null;
        }

        static public bool GetAdminPermission(string Login, string Pass)
        {
            using (AdministratorContext db = new AdministratorContext())
            {
                var adm = db.Admins.Where(a => a.Login == Login && a.Pass == Pass).FirstOrDefault();
                if (adm == null) return false;
                return adm.isSuperAdmin;
            }
        }

        static public bool AdminInput(string Login, string Password)
        {
            using (AdministratorContext db = new AdministratorContext())
                return db.Admins.Where(a => a.Login == Login && a.Pass == Password).FirstOrDefault() != null;
        }

        static public Student GetStudent(string StudentNumber)
        {
            using (StudentContext db = new StudentContext())
                return db.Students.Find(StudentNumber);
        }

        static public bool IsStudentNumberNotUsed(string StudentNumber)
        {
            return !StudentInput(StudentNumber);
        }

        static public List<SessionResult> GetSessions(string StudentNumber)
        {
            using (SessionContext db = new SessionContext())
                return db.Sessions.Where(s => s.Student.StudentNumber == StudentNumber).ToList();
        }

        static public Discipline GetDiscipline(int DisciplineID)
        {
            using (DisciplineContext db = new DisciplineContext())
                return db.Disciplines.Where(d => d.ID == DisciplineID).FirstOrDefault();
        }

        static public void StudentADD(Student student)
        {
            using (StudentContext db = new StudentContext())
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
        }

        static public void DisciplineADD(Discipline discipline)
        {
            using (DisciplineContext db = new DisciplineContext())
            {
                db.Disciplines.Add(discipline);
                db.SaveChanges();
            }
        }

        /// <returns>Returns true when administrator added correctly</returns>
        static public bool AdminADD(Administrator admin)
        {
            using (AdministratorContext db = new AdministratorContext())
            {
                if (db.Admins.Find(admin.Login) == null)
                {
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return true;
                } 
                else
                {
                    return false;
                }
            }
        }

        /// <returns>Returns true when session result added correctly</returns>
        static public bool SessionResultADD(string StudentNumber, SessionResult sessionResult)
        {
            using (StudentContext db = new StudentContext())
            {
                var student = db.Students.Find(StudentNumber);
                if (student != null)
                {
                    student.sessionResults.Add(sessionResult);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
