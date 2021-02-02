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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudSigns
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Database preload
            using (StudentContext StudentsDb = new StudentContext())
                StudentsDb.Students.Find("");
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
        private void StudentNumberTB_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Trim() == "") tb.Text = "Номер зачетной книги";
        }

        private void StudentNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || (e.Key > Key.D0 && e.Key < Key.D9) || e.Key == Key.Enter) && e.Key != Key.Space) return;
            e.Handled = true;
        }

        //Login buttons logic
        private void StudentLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBInterface.StudentInput(StudentNumberTB.Text.Trim()))
            {
                var StudWindow = new StudentWindow(StudentNumberTB.Text.Trim());
                StudWindow.Show();
            }
            else
            {
                MessageBox.Show(this,
                            $"Cтудента с номером зачетной книги \"{StudentNumberTB.Text}\" не существует!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }
        }
        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBInterface.AdminInput(AdminLoginTB.Text.Trim(), AdminPassTB.Password.Trim()))
            {
                var AdminWindow = new AdminWindow(AdminLoginTB.Text.Trim());
                AdminWindow.Show();
            } else
            {
                MessageBox.Show(
                        "Неверный логин или пароль!",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
            }
        }

    }
}
