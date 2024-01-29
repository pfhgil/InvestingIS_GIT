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
    /// Логика взаимодействия для AddCompanyWindow.xaml
    /// </summary>
    public partial class AddCompanyWindow : Window
    {
        public AddCompanyWindow()
        {
            InitializeComponent();
        }

        // todo: make
        private void AddCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyNameTextBox.Text.Length < 3 || CompanyNameTextBox.Text.Length > 48 ||
                !Utils.CyrillicRegex.IsMatch(CompanyNameTextBox.Text) || !Utils.EnglishRegex.IsMatch(CompanyNameTextBox.Text)) return;

            MainISWindow.CompaniesTableAdapter.InsertCompany(CompanyNameTextBox.Text);

            var mainISWindow = new MainISWindow();
            mainISWindow.Show();
            this.Close();
        }

        private void ReturnToMainISWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainISWindow = new MainISWindow();
            mainISWindow.Show();
            this.Close();
        }
    }
}
