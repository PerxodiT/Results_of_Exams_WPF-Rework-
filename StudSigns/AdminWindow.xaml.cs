using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            StudentEditor.Init();

            SAddMaleRB.IsChecked = true;
            SAddFemaleRB.IsChecked = false;
            DataLoad();
        }

        private void DataLoad()
        {
            StudentsDG.ItemsSource = new List<Student>();
            StudentsDG.ItemsSource = StudentEditor.GetStudents();
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

        //Textbox logic
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text == "Логин" || tb.Text == "Номер зачетной книги")
                tb.Text = "";
        }
        private void AdminPassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            if (pb.Password == "Пароль") pb.Password = "";
        }
        private void AdminPassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            if (pb.Password.Trim() == "") pb.Password = "Пароль";
        }
        private void AdminLoginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Trim() == "") tb.Text = "Логин";
        }

        private void LRC_RightButton_Click(object sender, RoutedEventArgs e)
        {
            int Number = Convert.ToInt32(LRC_Label.Text);
            Number = (Number + 1) % 11;
            Number = Number == 0 ? 1 : Number;
            LRC_Label.Text = Number.ToString();
        }

        private void LRC_LeftButton_Click(object sender, RoutedEventArgs e)
        {
            int Number = Convert.ToInt32(LRC_Label.Text);
            Number -= 1;
            Number = Number <= 0 ? 10 : Number;
            LRC_Label.Text = Number.ToString();
        }

        private void SAddBTN_Click(object sender, RoutedEventArgs e)
        {
            if (SAddSNameTB.Text.Trim() != "" && SAddSNumberTB.Text.Trim() != "" && SAddSpecialtyTB.Text.Trim() != "" && SAddFacultyTB.Text.Trim() != "" && SAddGroupTB.Text.Trim() != "" && SAddMaleRB.IsChecked != null && SAddFemaleRB != null && SAddBirthdayDP.SelectedDate != null)
            {
                Student stud;
                if (DBInterface.IsStudentNumberNotUsed(SAddSNumberTB.Text.Trim())) {
                    stud = new Student()
                    {
                        Name = SAddSNameTB.Text.Trim(),
                        StudentNumber = SAddSNumberTB.Text.Trim(),
                        Specialty = SAddSpecialtyTB.Text.Trim(),
                        Faculty = SAddFacultyTB.Text.Trim(),
                        DateOfBirth = (DateTime)SAddBirthdayDP.SelectedDate,
                        Gender = SAddMaleRB.IsChecked == true ? "Мужской" : "Женский",
                        Group = SAddGroupTB.Text.Trim()
                    };
                    DBInterface.StudentADD(stud);
                    DataLoad();
                    MessageBox.Show(this,
                            $"Студент добавлен!",
                            "Успех!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                }
                else
                {
                    MessageBox.Show(this,
                            $"Студент с таким номером зачетной книги уже существует!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                }
               
            } else
            {
                MessageBox.Show(this,
                            $"Не все обязательные поля заполнены!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );

                if (SAddSNameTB.Text == "")
                    NameLB.Foreground = Brushes.Red;
                else
                    NameLB.Foreground = Brushes.White;

                if (SAddSNumberTB.Text == "")
                    SNumberLB.Foreground = Brushes.Red;
                else
                    SNumberLB.Foreground = Brushes.White;

                if (SAddSpecialtyTB.Text == "")
                    SpecialtyLB.Foreground = Brushes.Red;
                else
                    SpecialtyLB.Foreground = Brushes.White;

                if (SAddFacultyTB.Text == "")
                    FacultyLB.Foreground = Brushes.Red;
                else
                    FacultyLB.Foreground = Brushes.White;

                if (SAddBirthdayDP.SelectedDate == null)
                    BirthdayLB.Foreground = Brushes.Red;
                else
                    BirthdayLB.Foreground = Brushes.White;

                if (SAddGroupTB.Text == "")
                    GroupLB.Foreground = Brushes.Red;
                else
                    GroupLB.Foreground = Brushes.White;
            }

        }

        private string EditingDateOfBirthText = "";
        private void StudentsBD_GotFocus(object sender, RoutedEventArgs e)
        {
            EditingDateOfBirthText = ((TextBox)sender).Text.Trim();
        }

        private void StudentsBD_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;

            Regex regex = new Regex(@"^(0[1-9]|[12][0-9]|3[01]).(0[1-9]|1[012]).(14|15|16|17|18|19|20)\d\d$");
            if (regex.IsMatch(tb.Text.Trim()))
            {

                return;
            }
            else
            {
                tb.Text = EditingDateOfBirthText;
                ((Student)StudentsDG.SelectedItem).DateOfBirth = Convert.ToDateTime(tb.Text);
            }
        }

        private void StudentsSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudentEditor.SaveChanges();
                MessageBox.Show(this,
                                $"Сохраниние успешно!",
                                "Успех!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                            );
            }
            catch (Exception)
            {
                MessageBox.Show(this,
                            $"Ошибка при сохранении!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }
        }

        private void StudentsUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void StudentsName_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Student)StudentsDG.SelectedItem).Name = ((TextBox)sender).Text.Trim();
        }

        private void StudentsGender_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Student)StudentsDG.SelectedItem).Gender = ((TextBox)sender).Text.Trim();
        }

        private void StudentsGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Student)StudentsDG.SelectedItem).Group = ((TextBox)sender).Text.Trim();
        }

        private void StudentsSpecialty_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Student)StudentsDG.SelectedItem).Specialty = ((TextBox)sender).Text.Trim();
        }

        private void StudentsFaculty_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Student)StudentsDG.SelectedItem).Faculty = ((TextBox)sender).Text.Trim();
        }

        private void StudentsDG_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && StudentsDG.SelectedItem != null)
            {
                var res = MessageBox.Show(this,
                    $"Вы действительно хотите удалить {((Student)StudentsDG.SelectedItem).Name}?",
                    "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    StudentsDG.CanUserDeleteRows = true;
                }
                else
                {
                    StudentsDG.CanUserDeleteRows = false;
                    e.Handled = true;
                }

            }
        }

        //
    }
}
