using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App440Project.Classes;

namespace App440Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayBill : ContentPage
    {
        private string passedBill;
        private double passedAmount;

        UpdateAllData payBill = new UpdateAllData();

        public string PassedBill
        {
            get { return passedBill; }
            set
            {
                passedBill = value;
                OnPropertyChanged(nameof(PassedBill));
            }
        }
        public double PassedAmount
        {
            get { return passedAmount; }
            set
            {
                passedAmount = value;
                OnPropertyChanged(nameof(PassedAmount));
            }
        }

        public PayBill(string newBill, double newAmount)
        {

            InitializeComponent();

            BindingContext = this;

            payBill.InSufficientFunds += () => DisplayAlert("Error", @"The account you selected does not have sufficient funds to pay this bill.", "Ok");

            PassedBill = newBill;
            PassedAmount = newAmount;

            pickerAccountType.ItemsSource = new List<string> {
                "Checking",
                "Savings"
            };
        }

        private void payBillButton_Clicked(object sender, EventArgs e)
        { 

            if (pickerAccountType.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an account type.", "Ok");
            }
            else
            {
                string selectedItem = pickerAccountType.Items[pickerAccountType.SelectedIndex];

                payBill.PayBill(selectedItem, PassedAmount, PassedBill);
                payBill.InsertTransactionRecords("Bill Paid", selectedItem, PassedAmount);

                if (UpdateAllData.insufficientAmount)
                {
                    return;
                }
                else
                {
                    DisplayAlert("Success", "Bill paid successfully.", "Ok");

                    //"Refresh" data by removing the old page and pushing a new page
                    Navigation.PopAsync();
                    Navigation.PushAsync(new TabbedPageMain());
                }


            }

        }
    }
}