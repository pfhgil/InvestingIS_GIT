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
    /// Логика взаимодействия для SecurityPaperCreateWindow.xaml
    /// </summary>
    public partial class SecurityPaperCreateWindow : Window
    {
        public SecurityPaperCreateWindow()
        {
            InitializeComponent();

            string[] items = new string[MainISWindow.CompaniesTableAdapter.GetData().Rows.Count];
            int i = 0;
            foreach (var row in MainISWindow.CompaniesTableAdapter.GetData().Rows)
            {
                items[i] = (string) MainISWindow.CompaniesTableAdapter.GetData().Rows[i]["company_name"];
                ++i;
            }

            CompanyComboBox.ItemsSource = items;
        }

        private void ReturnToMainISWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainISWindow = new MainISWindow();
            mainISWindow.Show();
            this.Close();
        }

        private void AddSecurityPaperButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecurityPaperNameTextBox.Text.Length < 3 || SecurityPaperNameTextBox.Text.Length > 24 ||
                !Utils.EnglishRegex.IsMatch(SecurityPaperNameTextBox.Text)) return;

            if (CompanyComboBox.SelectedItem != null)
            {
                decimal cost;
                bool parsed = decimal.TryParse(SecurityPaperStartCostTextBox.Text, out cost);

                if (!parsed) return;
                
                var company = MainISWindow.CompaniesTableAdapter.GetCompanyByName(CompanyComboBox.SelectedItem.ToString());

                MainISWindow.SecurityPapersTableAdapter.InsertSecurity(SecurityPaperNameTextBox.Text, cost, cost, (int) company.Rows[0]["company_id"]);

                var mainISWindow = new MainISWindow();
                mainISWindow.Show();
                this.Close();
            }
        }
    }
}
