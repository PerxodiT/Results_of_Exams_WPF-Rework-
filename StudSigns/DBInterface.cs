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
    }
}
