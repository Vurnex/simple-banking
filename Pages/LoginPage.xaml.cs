using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App440Project.ViewModels;

namespace App440Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            
            InitializeComponent();

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };

            vm.showAI += async () =>
            {
                ai.IsRunning = true;
                aiLayout.IsVisible = true;
                await Task.Delay(2000);
                aiLayout.IsVisible = false;
                ai.IsRunning = false;
            };

            vm.NavigateToMainPage += () => Navigation.PushAsync(new TabbedPageMain());
        }

    }
}