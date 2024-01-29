using InvestingIS.InvestingISDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
    /// Логика взаимодействия для MainISWindow.xaml
    /// </summary>
    public partial class MainISWindow : Window
    {
        public static CompaniesTableAdapter CompaniesTableAdapter { get; } = new CompaniesTableAdapter();
        public static BagsTableAdapter BagsTableAdapter { get; } = new BagsTableAdapter();
        public static TransactionsTableAdapter TransactionsTableAdapter { get; } = new TransactionsTableAdapter();
        public static SecurityPapersTableAdapter SecurityPapersTableAdapter { get; } = new SecurityPapersTableAdapter();

        public MainISWindow()
        {
            InitializeComponent();

            CompaniesDataGrid.ItemsSource = CompaniesTableAdapter.GetData();
            SB_SecurityPapersDataGrid.ItemsSource = SecurityPapersTableAdapter.GetData();

            BagsDataGrid.ItemsSource= BagsTableAdapter.GetData();
            IR_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            SB_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            TransactionsDataGrid.ItemsSource = TransactionsTableAdapter.GetData();

            UserLogin.Text = "Профиль пользователя " + CurrentUser.Login;
            UserFIO.Text = "ФИО: " + CurrentUser.FIO;
            UserID.Text = "ID: " + CurrentUser.ID.ToString();
        }

        private void AddCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            var addCompanyWindow = new AddCompanyWindow();
            addCompanyWindow.Show();
            this.Close();
        }

        private void DeleteCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompaniesDataGrid.SelectedItem != null)
            {
                int companyID = (int)((DataRowView)CompaniesDataGrid.SelectedItem).Row["company_id"];

                CompaniesTableAdapter.DeleteCompany(companyID);

                CompaniesDataGrid.ItemsSource = CompaniesTableAdapter.GetData();
                SB_SecurityPapersDataGrid.ItemsSource = SecurityPapersTableAdapter.GetData();
                TransactionsDataGrid.ItemsSource = TransactionsTableAdapter.GetData();
            }
        }

        private void AddBagButton_Click(object sender, RoutedEventArgs e)
        {
            var addBagWindow = new AddBagWindow();
            addBagWindow.Show();
            this.Close();
        }

        private void DeleteBagButton_Click(object sender, RoutedEventArgs e)
        {
            if (BagsDataGrid.SelectedItem != null)
            {
                int bagID = (int)((DataRowView)BagsDataGrid.SelectedItem).Row["bag_id"];

                BagsTableAdapter.DeleteBag(bagID);

                BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
                IR_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
                SB_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();

                TransactionsDataGrid.ItemsSource = TransactionsTableAdapter.GetData();
            }
        }

        private void AddSecurityPaperButton_Click(object sender, RoutedEventArgs e)
        {
            var securityPaperCreateWindow = new SecurityPaperCreateWindow();
            securityPaperCreateWindow.Show();
            this.Close();
        }

        private void IR_ExportBagButton_Click(object sender, RoutedEventArgs e)
        {
            if (IR_BagsDataGrid.SelectedItem != null)
            {
                var reportDateTime = DateTime.Now;

                string filename = (IR_CurrentChosenBagName.Text + "_EXPORT_" + reportDateTime)
                    .Replace(".", "_")
                    .Replace(":", "_") + ".docx";
                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }

                int bagID = (int) ((DataRowView)IR_BagsDataGrid.SelectedItem).Row["bag_id"];

                var foundBag = BagsTableAdapter.GetBagByID(bagID);

                string finalText = "Отчётность о портфеле " + foundBag.Rows[0]["bag_name"] + " на " + reportDateTime;

                finalText += "\n\nID: " + bagID;
                finalText += "\nНазвание: " + foundBag.Rows[0]["bag_name"];
                finalText += "\nБаланс: " + foundBag.Rows[0]["bag_balance"];
                finalText += "\nОбщая сумма транзакций: " + foundBag.Rows[0]["bag_transactions_total_cost"];
                finalText += "\nID владельца портфеля: " + foundBag.Rows[0]["user_id"];

                finalText += "\n\n------------------------------- Транзакции -------------------------------\n";

                foreach(var row in TransactionsTableAdapter.GetData())
                {
                    if ((int) row["bag_id"] == bagID)
                    {
                        finalText += "\nID: " + row["transaction_id"];
                        finalText += "\nID портфеля, с которого была проведена транзакция: " + row["bag_id"];
                        finalText += "\nДата транзакции: " + row["transaction_date"];
                        finalText += "\nТип транзакции: " + row["transaction_type"];
                        finalText += "\nID ценной бумаги: " + row["security_id"];
                        finalText += "\nКол-во ценных бумаг: " + row["security_papers_count"];
                        finalText += "\nОбщая стоимость транзакции: " + row["transaction_cost"];
                    }

                    finalText += "\n----------------\n";
                }

                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(Directory.GetCurrentDirectory() + "\\" + filename);
                // app.Visible = false;  
                doc.Content.Text = finalText;
                doc.Save();

                app.Quit();
            }
        }

        public void CurrentUserRoleChanged()
        {
            if(CurrentUser.Role == "Инвестор")
            {
                AddCompanyButton.Visibility = Visibility.Hidden;
                DeleteCompanyButton.Visibility = Visibility.Hidden;

                AddSecurityPaperButton.Visibility = Visibility.Hidden;

                CompaniesDataGrid.IsReadOnly = true;
                SB_SecurityPapersDataGrid.IsReadOnly = true;
            }
        }

        private void CompaniesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompaniesDataGrid.SelectedItem != null && CompaniesDataGrid.SelectedItem is DataRowView)
            {
                CurrentChosenCompanyName.Text = ((DataRowView)CompaniesDataGrid.SelectedItem).Row["company_name"].ToString();
            }
        }

        private void CompaniesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            int companyID = (int)((DataRowView)CompaniesDataGrid.SelectedItem).Row["company_id"];
            string companyName = ((DataRowView)CompaniesDataGrid.SelectedItem).Row["company_name"].ToString();
            
            CompaniesTableAdapter.UpdateCompany(companyName, companyID);

            CompaniesDataGrid.ItemsSource = CompaniesTableAdapter.GetData();
        }

        private void BagsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BagsDataGrid.SelectedItem != null)
            {
                CurrentChosenBagID.Text = "ID: " + ((DataRowView)BagsDataGrid.SelectedItem).Row["bag_id"].ToString();
                CurrentChosenBagName.Text = ((DataRowView)BagsDataGrid.SelectedItem).Row["bag_name"].ToString();
                TotalTransactionsCost.Text = "Общая сумма транзакций: " + ((DataRowView)BagsDataGrid.SelectedItem).Row["bag_transactions_total_cost"].ToString();
                BagBalance.Text = "Баланс: " + ((DataRowView)BagsDataGrid.SelectedItem).Row["bag_balance"].ToString();
            }
        }

        private void BagsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            int bagID = (int)((DataRowView)BagsDataGrid.SelectedItem).Row["bag_id"];
            string bagName = ((DataRowView)BagsDataGrid.SelectedItem).Row["bag_name"].ToString();
            decimal bagBalance = (decimal)((DataRowView)BagsDataGrid.SelectedItem).Row["bag_balance"];
            decimal bagTransactionsTotalCost = (decimal)((DataRowView)BagsDataGrid.SelectedItem).Row["bag_transactions_total_cost"];

            BagsTableAdapter.UpdateBag(bagName, bagBalance, bagTransactionsTotalCost, bagID);

            BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            IR_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            SB_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
        }

        private void SB_BagsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SB_BagsDataGrid.SelectedItem != null)
            {
                SB_CurrentChosenBagName.Text = "Текущий портфель: " + ((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_name"].ToString();
                SB_CurrentChosenBagBalance.Text = "Баланс: " + ((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_balance"].ToString();
            }
        }

        private void SB_SecurityPapersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SB_SecurityPapersDataGrid.SelectedItem != null)
            {
                SB_CurrentSecurityName.Text = "Текущая выбранная ценная бумага: " + ((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_name"].ToString();
                SB_CurrentSecurityPaperCost.Text = "Стоимость: " + ((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_cost"].ToString();
            }
        }

        private void SB_SecurityPapersDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            int securityID = (int)((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_id"];
            string securityName = ((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_name"].ToString();
            decimal securityCost = (decimal)((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_cost"];

            SecurityPapersTableAdapter.UpdateSecurityByID(securityName, securityCost, securityID);

            SB_SecurityPapersDataGrid.ItemsSource = SecurityPapersTableAdapter.GetData();
        }

        private void BuySecurityPapersButton_Click(object sender, RoutedEventArgs e)
        {
            int securityPapersCount;
            bool parsed = int.TryParse(SB_SecurityPapersCountTextBox.Text, out securityPapersCount);
            if (!(parsed && SB_SecurityPapersDataGrid.SelectedItem != null && SB_BagsDataGrid.SelectedItem != null)) return;

            int bagID = (int)((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_id"];
            string bagName = ((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_name"].ToString();
            decimal bagBalance = (decimal)((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_balance"];
            decimal bagTransactionsTotalCost = (decimal)((DataRowView)SB_BagsDataGrid.SelectedItem).Row["bag_transactions_total_cost"];

            int securityID = (int)((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_id"];
            string securityName = ((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_name"].ToString();
            decimal securityCost = (decimal)((DataRowView)SB_SecurityPapersDataGrid.SelectedItem).Row["security_cost"];

            BagsTableAdapter.UpdateBag(bagName, bagBalance - securityCost * securityPapersCount, bagTransactionsTotalCost + securityCost * securityPapersCount, bagID);

            TransactionsTableAdapter.InsertTransaction(bagID, DateTime.Now, "Покупка", securityID, securityPapersCount, securityCost * securityPapersCount);

            TransactionsDataGrid.ItemsSource = TransactionsTableAdapter.GetData();

            BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            IR_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
            SB_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();
        }

        private void TransactionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionsDataGrid.SelectedItem != null)
            {
                DateTime transactionDate = (DateTime) ((DataRowView)TransactionsDataGrid.SelectedItem).Row["transaction_date"];
                int transactionID = (int)((DataRowView)TransactionsDataGrid.SelectedItem).Row["transaction_id"];
                int securityID = (int)((DataRowView)TransactionsDataGrid.SelectedItem).Row["security_id"];
                int securityPapersCount = (int)((DataRowView)TransactionsDataGrid.SelectedItem).Row["security_papers_count"];
                decimal transcationCost = (decimal)((DataRowView)TransactionsDataGrid.SelectedItem).Row["transaction_cost"];
                decimal securityOpenCost = (decimal)SecurityPapersTableAdapter.GetSecurityByID(securityID)[0]["security_open_cost"];
                decimal securityCloseCost = (decimal)SecurityPapersTableAdapter.GetSecurityByID(securityID)[0]["security_close_cost"];

                var transactionViewWindow = new TransactionViewWindow();
                transactionViewWindow.setSecurityData(transactionDate, securityID, securityPapersCount, transcationCost, securityOpenCost, securityCloseCost);
                transactionViewWindow.Show();
            }
        }

        private void IR_BagsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IR_BagsDataGrid.SelectedItem != null)
            {
                IR_CurrentChosenBagID.Text = "ID: " + ((DataRowView)IR_BagsDataGrid.SelectedItem).Row["bag_id"].ToString();
                IR_CurrentChosenBagName.Text = ((DataRowView)IR_BagsDataGrid.SelectedItem).Row["bag_name"].ToString();
            }
        }

        private void CompanyNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            CompaniesDataGrid.ItemsSource = CompaniesTableAdapter.GetData();

            List<object> collectedElems = new List<object>();

            foreach(DataRowView row in CompaniesDataGrid.Items)
            {
                if (((string) row["company_name"]).StartsWith(CompanyNameTextBox.Text))
                {
                    collectedElems.Add(row);
                }
            }

            CompaniesDataGrid.ItemsSource = collectedElems;
        }

        private void BagNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();

            List<object> collectedElems = new List<object>();

            foreach (DataRowView row in BagsDataGrid.Items)
            {
                if (((string)row["bag_name"]).StartsWith(BagNameTextBox.Text))
                {
                    collectedElems.Add(row);
                }
            }

            BagsDataGrid.ItemsSource = collectedElems;
        }

        private void SB_BagNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SB_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();

            List<object> collectedElems = new List<object>();

            foreach (DataRowView row in SB_BagsDataGrid.Items)
            {
                if (((string)row["bag_name"]).StartsWith(SB_BagNameTextBox.Text))
                {
                    collectedElems.Add(row);
                }
            }

            SB_BagsDataGrid.ItemsSource = collectedElems;
        }

        private void SB_SecurityNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SB_SecurityPapersDataGrid.ItemsSource = SecurityPapersTableAdapter.GetData();

            List<object> collectedElems = new List<object>();

            foreach (DataRowView row in SB_SecurityPapersDataGrid.Items)
            {
                if (((string)row["security_name"]).StartsWith(SB_SecurityNameTextBox.Text))
                {
                    collectedElems.Add(row);
                }
            }

            SB_SecurityPapersDataGrid.ItemsSource = collectedElems;
        }

        private void IR_BagNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            IR_BagsDataGrid.ItemsSource = BagsTableAdapter.GetData();

            List<object> collectedElems = new List<object>();

            foreach (DataRowView row in IR_BagsDataGrid.Items)
            {
                if (((string)row["bag_name"]).StartsWith(IR_BagNameTextBox.Text))
                {
                    collectedElems.Add(row);
                }
            }

            IR_BagsDataGrid.ItemsSource = collectedElems;
        }
    }
}
