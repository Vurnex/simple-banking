using App440Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

using App440Project.Pages;
using App440Project.Classes;

namespace App440Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillsPage : ContentPage
    {
        ObservableCollection<BillDetails> bills = new ObservableCollection<BillDetails>();

        MySqlConnection conn;

        string billTitle;
        double billAmount;

        public BillsPage()
        {
            InitializeComponent();

            BindingContext = new BillsPageViewModel();

            myListView.ItemsSource = bills;
            noBills.IsVisible = false;

            try
            {
                conn = DBUtils.CreateConnection();
                conn.Open();

                MySqlDataReader billsData = DBUtils.getBills(conn);

                while (billsData.Read())
                {
                    billAmount = (double)billsData["BillAmount"];
                    billTitle = (string)billsData["BillTitle"];

                    bills.Add(new BillDetails() { BillTitle = billTitle, BillAmount = billAmount });
                };

            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }

            if (bills.Count < 1)
            {
                noBills.IsVisible = true;
            }

        }

        private void myListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            BillDetails item = (BillDetails)e.Item;

            string itemTitle = item.BillTitle.ToString();
            double itemNum = item.BillAmount;

            Navigation.PushAsync(new PayBill(itemTitle, itemNum));

            //DisplayAlert("Item Info", item.AccountType.ToString(), "Ok");

        }

    }
}