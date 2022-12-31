using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App440Project.Pages;

namespace App440Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage : ContentPage
    {
        public TransferPage()
        {
            InitializeComponent();
        }

        public async void transferButton_Clicked(object sender, EventArgs e)
        {
            //Add animation to logout button
            await transferButton.ScaleTo(0.75, 100);
            await transferButton.ScaleTo(1, 100);

            await Navigation.PushAsync(new MakeTransfer());
        }
    }
}