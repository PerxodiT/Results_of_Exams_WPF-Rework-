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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudSigns
{
    /// <summary>
    /// Логика взаимодействия для AddAdminTab.xaml
    /// </summary>
    public partial class AddAdminTab : Page
    {
        private AdminWindow AdminWindow;
        public AddAdminTab(AdminWindow adminWindow)
        {
            InitializeComponent();
            AdminWindow = adminWindow;
        }

        private void DataLoad()
        {
            AdminsDG.ItemsSource = AdminEditor.GetAdmins();
        }

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

        private void AdminsUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void AdminsSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AdminEditor.SaveChanges();
                MessageBox.Show(AdminWindow,
                                $"Сохраниние успешно!",
                                "Успех!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                            );
            }
            catch (Exception)
            {
                MessageBox.Show(AdminWindow,
                            $"Ошибка при сохранении!",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }
        }

        private void AdminsPasswords_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex passwordCheck = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{6,16}$");
            string pass = ((PasswordBox)sender).Password.Trim();
            if (!passwordCheck.IsMatch(pass))
            {
                MessageBox.Show(
                            "Ошибка",
                            "Слишком слабый пароль, в пароле должна быть минимум \n" +
                            "одна цифра, одна буква(английская),\n" +
                            "большая буква и любой знак (? = .), \n" +
                            "длина пароля от 6 до 16 символов!\n",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                ((PasswordBox)sender).Password = "Пароль";
                return;
            }
            else if (((PasswordBox)sender).Password.Trim() != "")
            {
                ((Administrator)AdminsDG.SelectedItem).Pass = ((PasswordBox)sender).Password.Trim();
            }
        }

        private void AdminsLogins_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Administrator)AdminsDG.SelectedItem).Login = ((TextBox)sender).Text.Trim();
        }

        private void AdminsDG_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && AdminsDG.SelectedItem != null)
            {
                var res = MessageBox.Show(AdminWindow,
                    $"Вы действительно хотите удалить {((Administrator)AdminsDG.SelectedItem).Login}?",
                    "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    AdminsDG.CanUserDeleteRows = true;
                }
                else
                {
                    AdminsDG.CanUserDeleteRows = false;
                    e.Handled = true;
                }
            }
        }

        private void AdminAddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
