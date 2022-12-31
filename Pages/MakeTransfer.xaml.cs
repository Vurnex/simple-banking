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
    public partial class MakeTransfer : ContentPage
    {
        string pickerOneItem;
        string pickerTwoItem;

        UpdateAllData sendData = new UpdateAllData();

        public MakeTransfer()
        {
            InitializeComponent();

            sendData.InSufficientFunds += () => DisplayAlert("Error", @"The amount you wish to transfer is greater than the current balance of this account.", "Ok");

            pickerAccountOne.ItemsSource = new List<string> {
                "Checking",
                "Savings"
            };

            pickerAccountTwo.ItemsSource = new List<string> {
                "Checking",
                "Savings"
            };

        }

        private void transferNowButton_Clicked(object sender, EventArgs e)
        {

            if (entryTransferAmount.Text == null || entryTransferAmount.Text == "")
            {
                DisplayAlert("Error", "Please enter an amount to be deposited.", "Ok");
            }
            else if (entryTransferAmount.Text == "-" || entryTransferAmount.Text == "." ||
                    entryTransferAmount.Text == "-." || entryTransferAmount.Text == ".-")
            {
                DisplayAlert("Error", "Please enter a valid amount.", "Ok");
            }
            else if (pickerOneItem == null || pickerTwoItem == null)
            {
                DisplayAlert("Error", "Please pick an account first.", "Ok");
            }
            else if (pickerOneItem == pickerTwoItem)
            {
                DisplayAlert("Error", "You can't transfer to the same account.", "Ok");
            }
            else
            {
                string transferAmount = entryTransferAmount.Text;

                double newTransferAmount = Convert.ToDouble(transferAmount);

                bool negative = newTransferAmount < 0;

                if (negative)
                {
                    DisplayAlert("Error", "You can't transfer a negative amount.", "Ok");
                    return;
                }

                sendData.UpdateWithTransferData(pickerOneItem, newTransferAmount);
                sendData.InsertTransactionRecords("Transfer", pickerOneItem, newTransferAmount);


                if (UpdateAllData.insufficientAmount)
                {
                    return;
                }
                else
                {
                    DisplayAlert("Success", "Transfer successful.", "Ok");

                    //"Refresh" data by removing the old page and pushing a new page
                    Navigation.PopAsync();
                    Navigation.PushAsync(new TabbedPageMain());
                }



            }
        }

        private void pickerAccountOne_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickerOneItem = pickerAccountOne.Items[pickerAccountOne.SelectedIndex];
        }

        private void pickerAccountTwo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickerTwoItem = pickerAccountTwo.Items[pickerAccountTwo.SelectedIndex];
        }
    }
}