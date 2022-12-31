using App440Project.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App440Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageMain : TabbedPage
    {
        public TabbedPageMain()
        {
            InitializeComponent();

            this.Title = "Simple Banking";

            this.Children.Add(new AccountPage() { Title = "Accounts" });
            this.Children.Add(new TransferPage() { Title = "Transfer" });
            this.Children.Add(new BillsPage() { Title = "Pay Bills" });
            this.Children.Add(new DepositPage() { Title = "Deposit" });

        }

        public async void OnImageButtonClicked(object sender, EventArgs e)
        {
            //Add animation to logout button
            await logoutButton.ScaleTo(0.75, 100);
            await logoutButton.ScaleTo(1, 100);

            //Clear the navigation stack
            await Navigation.PopToRootAsync();
        }
    }
}