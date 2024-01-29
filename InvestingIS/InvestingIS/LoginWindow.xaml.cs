using InvestingIS.InvestingISDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace InvestingIS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public UsersTableAdapter UsersTableAdapter { get; } = new UsersTableAdapter();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text.Length < 3 || LoginTextBox.Text.Length > 24 || (!Utils.CyrillicRegex.IsMatch(LoginTextBox.Text) && !Utils.EnglishRegex.IsMatch(LoginTextBox.Text))) return;

            if (PasswordTextBox.Password.Length < 3 || PasswordTextBox.Password.Length > 20 ||
                Utils.EnglishRegex.Matches(PasswordTextBox.Password).Count == 0 ||
                Utils.SpecSymbolsRegex.Matches(PasswordTextBox.Password).Count < 2 ||
                Utils.NumbersRegex.Matches(PasswordTextBox.Password).Count < 2) return;

            var foundUser = UsersTableAdapter.GetData()[(int) UsersTableAdapter.GetUserByLogin(LoginTextBox.Text) - 1];

            if (foundUser == null) return;

            CurrentUser.ID = (int)foundUser["user_id"];
            CurrentUser.Role = (string) foundUser["user_role"];
            CurrentUser.FIO = (string) foundUser["user_surname"] + " " + (string) foundUser["user_name"] + " " + (string) foundUser["user_patronymic"];
            CurrentUser.Login = (string) foundUser["user_login"];

            var mainISWindow = new MainISWindow();
            mainISWindow.Show();

            mainISWindow.CurrentUserRoleChanged();

            this.Close();
        }

        private void GotoRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
