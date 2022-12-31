using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace App440Project.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public Action showAI;
        public Action NavigateToMainPage;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public void OnSubmit()
        {
            if (email != "440@test.com" || password != "secret")
            {
                DisplayInvalidLoginPrompt();
            }
            else
            {
                showAI();
                NavigateToMainPage();
            }
        }
    }
}
