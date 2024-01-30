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
    /// Логика взаимодействия для AddBagWindow.xaml
    /// </summary>
    public partial class AddBagWindow : Window
    {
        public AddBagWindow()
        {
            InitializeComponent();
        }

        private void ReturnToMainISWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var mainISWindow = new MainISWindow();
            mainISWindow.Show();
            this.Close();
        }

        private void AddBagButton_Click(object sender, RoutedEventArgs e)
        {
            if (BagNameTextBox.Text.Length < 3 || BagNameTextBox.Text.Length > 24 ||
                (!Utils.CyrillicRegex.IsMatch(BagNameTextBox.Text) && !Utils.EnglishRegex.IsMatch(BagNameTextBox.Text))) return;

            MainISWindow.BagsTableAdapter.InsertBag(BagNameTextBox.Text, CurrentUser.ID);

            var mainISWindow = new MainISWindow();
            mainISWindow.Show();
            this.Close();
        }
    }
}
