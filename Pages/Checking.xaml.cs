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
    public partial class Checking : ContentPage
    {

        MySqlConnection conn;

        ObservableCollection<CheckingInfo> checkingT = new ObservableCollection<CheckingInfo>();

        string transaction;
        double amount;
        public static double totalAmount;

        public Checking()
        {
            InitializeComponent();

            myListView.ItemsSource = checkingT;

            try
            {
                conn = DBUtils.CreateConnection();
                conn.Open();

                MySqlDataReader checkingData = DBUtils.getCheckingTransactions(conn);

                while (checkingData.Read())
                {
                    amount = (double)checkingData["Amount"];
                    transaction = (string)checkingData["Transaction"];
                    
                    checkingT.Add(new CheckingInfo() { TransactionAmount = amount, TransactionName = transaction });

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