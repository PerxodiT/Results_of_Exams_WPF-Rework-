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
    public delegate void OnConfirmEvent();
    /// <summary>
    /// Логика взаимодействия для PasswordConfirmation.xaml
    /// </summary>
    public partial class PasswordConfirmation : Page
    {
        public OnConfirmEvent OnConfirm;
        private string AdminLogin;
        public PasswordConfirmation(string login, OnConfirmEvent onConfirm)
        {
            InitializeComponent();
            OnConfirm = onConfirm;
            AdminLogin = login;
        }

        private void PassTB_GotFocus(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            if (pb.Password == "Пароль") pb.Password = "";
        }

        private void PassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            if (pb.Password.Trim() == "") pb.Password = "Пароль";
        }

        private void AddAdminConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBInterface.AdminInput(AdminLogin, Pass.Password.Trim()))
                OnConfirm?.Invoke();
            else
                MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
