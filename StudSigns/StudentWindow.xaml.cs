using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudSigns
{
    /// <summary>
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private Student Stud; //Authentificated student
        private List<SessionResult> Sessions;

        public StudentWindow(string StudentNumber)
        {
            InitializeComponent();

            DisciplineNameLB.Text = "";
            TeacherNameLB.Text = "";
            FillStudentInfo(StudentNumber);
            LoadData();
        }

        private void ResultsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsDG.SelectedItem != null)
            {
                try
                {
                    var Did = ((SessionResult)ResultsDG.SelectedItem).DisciplineID;
                    var Discipline = DBInterface.GetDiscipline(Did);
                    DisciplineNameLB.Text = Discipline.Name;
                    TeacherNameLB.Text = Discipline.Teacher;
                }
                catch (System.InvalidCastException)
                {
                    return;
                }
            }
        }

        private void FillStudentInfo(string StudentNumber)
        {
            Stud = DBInterface.GetStudent(StudentNumber);

            StudNumPole.Text = Stud.StudentNumber;
            StudNamePole.Text = Stud.Name;
            StudSpecialtyPole.Text = Stud.Specialty;
            StudFacultyPole.Text = Stud.Faculty;
            StudGenderPole.Text = Stud.Gender;
            StudGroupPole.Text = Stud.Group;
            StudBirthdayPole.Text = Stud.DateOfBirth.ToString("d");
        }

        private void LoadData()
        {
            Sessions = DBInterface.GetSessions(Stud.StudentNumber);
            ResultsDG.ItemsSource = Sessions;
        }

        //System components logic
        private void TopCase_DragMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinimazeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Normal:
                    this.WindowState = WindowState.Maximized;
                    break;
                default:
                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;

            }
        }
    }
}
