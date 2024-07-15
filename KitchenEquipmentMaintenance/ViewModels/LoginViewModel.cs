using KitchenEquipmentMaintenance.Helpers;
using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace KitchenEquipmentMaintenance.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        private string _userName;
        private string _password;
        private string _loginStatus;

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string LoginStatus
        {
            get => _loginStatus;
            set { _loginStatus = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get;}
        public ICommand SignUpCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            SignUpCommand = new RelayCommand(SignUp);
        }

        private void Login()
        {
            using (var context = new AppDbContext())
            {
                var hashedPassword = PasswordHelper.HashPassword(Password);

                var user = context.Users.FirstOrDefault(u => u.UserName == UserName && u.Password == hashedPassword);
                if (user != null)
                {

                    StoreCurrentUser(user);
                    LoginStatus = "Login Successful!";
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var adminView = new AdminView();
                        adminView.Show();
                        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView)?.Close();

                    });

                }
                else
                {
                    LoginStatus = "Invalid Username or Password!";

                }
            }
        }
        private void StoreCurrentUser(User user)
        {
            App.CurrentUser = user;                               
        }

        private void SignUp()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var signUpView = new SignUpView();
                signUpView.Show();
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView)?.Close();

            });

        }
      
    }
}
