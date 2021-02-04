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
        private string LoggedAdminLogin;
        private AddAdminTab AddAdminTab;

        public AdminWindow(string AdminLogin, string AdminPass)
        {
            InitializeComponent();
            StudentEditor.Init();
            AdminEditor.Init();
            DisciplineEditor.Init();
            LoggedAdminLogin = AdminLogin;
            AddAdminTab = new AddAdminTab(this);

            if (!DBInterface.GetAdminPermission(AdminLogin, AdminPass))
            {
                AddAdminTabItem.IsEnabled = false;
            }
            SAddMaleRB.IsChecked = true;
            SAddFemaleRB.IsChecked = false;
            DataLoad();
        }


        private void DataLoad()
        {
            StudentsDG.ItemsSource = StudentEditor.GetStudents();
            DisciplinesDG.ItemsSource = DisciplineEditor.GetDisciplines();
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
                ((Student)StudentsDG.SelectedItem).DateOfBirth = Convert.ToDateTime(tb.Text.Trim());
                return;
            }
            else
            {
                tb.Text = EditingDateOfBirthText;
                ((Student)StudentsDG.SelectedItem).DateOfBirth = Convert.ToDateTime(tb.Text.Trim());
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
            if (((TextBox)sender).Text.Trim() != "")
            ((Student)StudentsDG.SelectedItem).Name = ((TextBox)sender).Text.Trim();
        }

        private void StudentsGender_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
            ((Student)StudentsDG.SelectedItem).Gender = ((TextBox)sender).Text.Trim();
        }

        private void StudentsGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
            ((Student)StudentsDG.SelectedItem).Group = ((TextBox)sender).Text.Trim();
        }

        private void StudentsSpecialty_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
            ((Student)StudentsDG.SelectedItem).Specialty = ((TextBox)sender).Text.Trim();
        }

        private void StudentsFaculty_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
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

        private void OnPasswordConfirmed()
        {
            AddAdminFrame.Navigate(AddAdminTab);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && this.IsLoaded)
            {
                TabItem selectedTab = e.AddedItems[0] as TabItem;
                if (selectedTab == null) return;

                if (selectedTab.Name == "AddAdminTabItem")
                {
                    AddAdminFrame.Navigate(new PasswordConfirmation(LoggedAdminLogin, OnPasswordConfirmed));
                }
            }
        }

        private void SaveDisciplinesBTN_Click(object sender, RoutedEventArgs e)
        {
            DisciplineEditor.SaveChanges();
        }

        private void AddDisciplineBTN_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (DisciplineNameTB.Text.Trim() == "")
            {
                DisciplineNameLB.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                DisciplineNameLB.Foreground = Brushes.White;
            }
            if (TeacherTB.Text.Trim() == "")
            {
                TeacherLB.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                TeacherLB.Foreground = Brushes.White;
            }
            if (isValid)
            {
                var Discipline = new Discipline()
                {
                    Name = DisciplineNameTB.Text.Trim(),
                    Teacher = TeacherTB.Text.Trim()
                };
                DBInterface.DisciplineADD(Discipline);
            }

        }

        private void DisciplinesDG_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && DisciplinesDG.SelectedItem != null)
            {
                var res = MessageBox.Show(this,
                    $"Вы действительно хотите удалить {((Discipline)DisciplinesDG.SelectedItem).Name}?",
                    "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    DisciplinesDG.CanUserDeleteRows = true;
                }
                else
                {
                    DisciplinesDG.CanUserDeleteRows = false;
                    e.Handled = true;
                }
            }
        }

        private void DisciplineUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void DisciplineName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
            ((Discipline)DisciplinesDG.SelectedItem).Name = ((TextBox)sender).Text.Trim();
        }

        private void Teacher_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "")
                ((Discipline)DisciplinesDG.SelectedItem).Teacher = ((TextBox)sender).Text.Trim();
        }

        private void AddSessionResultBTN_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (AddResultStudentNumberTB.Text.Trim() == "")
            {
                AddResultStudentNumberLB.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                AddResultStudentNumberLB.Foreground = Brushes.White;
            }
            if (ExamDateDP.SelectedDate == null)
            {
                ExamDateLB.Foreground = Brushes.Red;
                isValid = false;
            }
            else
            {
                ExamDateLB.Foreground = Brushes.White;
            }

            if (DisciplinesDG.SelectedItem == null)
            {
                MessageBox.Show(this,
                            $"Выберите дисциплину в таблице!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                return;
            }

            if (isValid)
            {
                var result = new SessionResult()
                {
                    DisciplineID = ((Discipline)DisciplinesDG.SelectedItem).ID,
                    ExamDate = (DateTime)ExamDateDP.SelectedDate,
                    ExamMark = Convert.ToInt16(LRC_Label.Text)
                };
                if (DBInterface.SessionResultADD(AddResultStudentNumberTB.Text.Trim(), result))
                {
                    MessageBox.Show(this,
                            "Результат добавлен!",
                            "Успех!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                }
                else
                {
                    MessageBox.Show(this,
                               $"Студента с таким номером зачетной книги не найдено!",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error
                           );
                }
            }
            else
            {
                MessageBox.Show(this,
                            $"Не все обязательные поля заполнены!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }
        }
    }
}
