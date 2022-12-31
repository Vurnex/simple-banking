using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

using App440Project.Classes;

namespace App440Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Savings : ContentPage
    {
        MySqlConnection conn;

        ObservableCollection<SavingsInfo> savingsT = new ObservableCollection<SavingsInfo>();

        string transaction;
        double amount;
        public static double totalAmount;

        public Savings()
        {
            InitializeComponent();

            myListView.ItemsSource = savingsT;

            try
            {
                conn = DBUtils.CreateConnection();
                conn.Open();

                MySqlDataReader savingsData = DBUtils.getSavingsTransactions(conn);

                while (savingsData.Read())
                {
                    amount = (double)savingsData["Amount"];
                    transaction = (string)savingsData["Transaction"];

                    savingsT.Add(new SavingsInfo() { TransactionAmount = amount, TransactionName = transaction });

                    totalAmount += amount;

                };

            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }

        }
    }
}