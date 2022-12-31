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
using App440Project.ViewModels;
using App440Project.Classes;


namespace App440Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        ObservableCollection<AccountInfo> accounts = new ObservableCollection<AccountInfo>();

        MySqlConnection conn;

        string accountType;
        double accountBalance;

        public AccountPage()
        {
            InitializeComponent();

            UpdateAllData updateNow = new UpdateAllData();

            updateNow.Update();

            BindingContext = new AccountPageViewModel();

            myListView.ItemsSource = accounts;

            try
            {
                conn = DBUtils.CreateConnection();
                conn.Open();

                MySqlDataReader checkingData = DBUtils.getAccounts(conn);

                while (checkingData.Read())
                {
                    accountBalance = (double)checkingData["AccountBalance"];
                    accountType = (string)checkingData["AccountType"];

                    accounts.Add(new AccountInfo() { AccountBalance = accountBalance, AccountType = accountType });
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

        private void myListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            AccountInfo item = (AccountInfo)e.Item;

            string itemTitle = item.AccountType.ToString();

            if (itemTitle == "Checking")
            {
                Navigation.PushAsync(new Checking());
            }
            else if (itemTitle == "Savings")
            {
                Navigation.PushAsync(new Savings());
            }

            //DisplayAlert("Item Info", item.AccountType.ToString(), "Ok");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
    }
}