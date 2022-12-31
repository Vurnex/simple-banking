using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App440Project.Classes;

namespace App440Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepositPage : ContentPage
    {
        string selectedItem;

        UpdateAllData depositCheck = new UpdateAllData();

        public DepositPage()
        {
            InitializeComponent();

            pickerAccountType.ItemsSource = new List<string> {
                "Checking",
                "Savings"
            };
        }

        private void pickerAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedItem = pickerAccountType.Items[pickerAccountType.SelectedIndex];
        }

        private void depositButton_Clicked(object sender, EventArgs e)
        {
            if (pickerAccountType.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an account type.", "Ok");
            }
            else if (entryDepositAmount.Text == null || entryDepositAmount.Text == "")

            {
                DisplayAlert("Error", "Please enter an amount to be deposited.", "Ok");
            }
            else if (entryDepositAmount.Text == "-" || entryDepositAmount.Text == "." ||
                    entryDepositAmount.Text == "-." || entryDepositAmount.Text == ".-")
            {
                DisplayAlert("Error", "Please enter a valid amount.", "Ok");
            }
            else
            {
                string depositAmount = entryDepositAmount.Text;

                double newDepositAmount = Convert.ToDouble(depositAmount);

                bool negative = newDepositAmount < 0;

                if (negative)
                {
                    DisplayAlert("Error", "You can't deposit a negative amount.", "Ok");
                    return;
                }

                UpdateAllData.insufficientAmount = false;

                depositCheck.DepositCheck("Deposit", selectedItem, newDepositAmount);

                DisplayAlert("Success", "Deposit successful.", "Ok");

                //"Refresh" data by removing the old page and pushing a new page
                Navigation.PopAsync();
                Navigation.PushAsync(new TabbedPageMain());

            }

        }
    }
}