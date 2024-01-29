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
    /// Логика взаимодействия для TransactionViewWindow.xaml
    /// </summary>
    public partial class TransactionViewWindow : Window
    {
        public TransactionViewWindow()
        {
            InitializeComponent();
        }

        public void setSecurityData(DateTime transactionDate, int securityID, int securityPapersCount, decimal transcationCost, decimal securityOpenCost, decimal securityCloseCost)
        {
            SecurityPaperOperationDate.Text = "Дата операции с ценной бумагой: " + transactionDate.ToString();
            SecurityPaperID.Text = "Идентификатор ценной бумаги: " + securityID.ToString();
            SecurityPapersCount.Text = "Количество ценной бумаги: " + securityPapersCount.ToString();
            TotalTransactionCost.Text = "Общая стоимость транзакции: " + transcationCost.ToString();
            SecurityPaperOpenCost.Text = "Цена открытия ценной бумаги: " + securityOpenCost.ToString();
            SecurityPaperCloseCost.Text = "Цена закрытия ценной бумаги: " + securityCloseCost.ToString();
        }

        private void ReturnToMainISWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
