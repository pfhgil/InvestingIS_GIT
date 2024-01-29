using InvestingIS.InvestingISDataSetTableAdapters;
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

namespace InvestingIS
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public UsersTableAdapter UsersTableAdapter { get; } = new UsersTableAdapter();

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Length < 1 || NameTextBox.Text.Length > 24 || !Utils.CyrillicRegex.IsMatch(NameTextBox.Text)) return;

            if (SurnameTextBox.Text.Length < 1 || SurnameTextBox.Text.Length > 24 || !Utils.CyrillicRegex.IsMatch(SurnameTextBox.Text)) return;

            if (PatronymicTextBox.Text.Length > 0 && (PatronymicTextBox.Text.Length > 24 || !Utils.CyrillicRegex.IsMatch(PatronymicTextBox.Text))) return;

            if (LoginTextBox.Text.Length < 3 || LoginTextBox.Text.Length > 24 || (!Utils.CyrillicRegex.IsMatch(LoginTextBox.Text) && !Utils.EnglishRegex.IsMatch(LoginTextBox.Text))) return;

            if (PasswordTextBox.Password.Length < 3 || PasswordTextBox.Password.Length > 20 || 
                Utils.EnglishRegex.Matches(PasswordTextBox.Password).Count == 0 || 
                Utils.SpecSymbolsRegex.Matches(PasswordTextBox.Password).Count < 2 ||
                Utils.NumbersRegex.Matches(PasswordTextBox.Password).Count < 2) return;

            if (PasswordTextBox.Password != PasswordRepeatTextBox.Password) return;

            UsersTableAdapter.InsertUser(SurnameTextBox.Text, NameTextBox.Text, PatronymicTextBox.Text, LoginTextBox.Text, PasswordTextBox.Password, "Инвестор");

            var foundUser = UsersTableAdapter.GetData()[(int)UsersTableAdapter.GetUserByLogin(LoginTextBox.Text) - 1];
            CurrentUser.ID = (int)foundUser["user_id"];
            CurrentUser.FIO = (string)foundUser["user_surname"] + " " + (string)foundUser["user_name"] + " " + (string)foundUser["user_patronymic"];
            CurrentUser.Login = (string)foundUser["user_login"];

            var mainISWindow = new MainISWindow();
            mainISWindow.Show();

            mainISWindow.CurrentUserRoleChanged();

            this.Close();
        }

        private void GotoAuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
