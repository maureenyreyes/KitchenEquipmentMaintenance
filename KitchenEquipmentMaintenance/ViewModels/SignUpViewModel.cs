using KitchenEquipmentMaintenance.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using KitchenEquipmentMaintenance.Views;
using KitchenEquipmentMaintenance.Views.User;
using KitchenEquipmentMaintenance.Helpers;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public string _firstName;
        public string _lastName;
        public string _emailAddress;
        public string _userName;
        public string _password;
        public string _signUpStatus;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }
        public string EmailAddress
        {
            get => _emailAddress;
            set { _emailAddress = value; OnPropertyChanged(); }
        }

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

        public string SignUpStatus
        {
            get => _signUpStatus;
            set { _signUpStatus = value; OnPropertyChanged(); }
        }

        public ICommand SignUpCommand { get; }
        public ICommand LoginCommand { get; }
        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(SignUp);
            LoginCommand = new RelayCommand(BackToLogin);

        }
        private void BackToLogin()
        {
            var loginViewModel = new LoginViewModel();
            var loginView = new LoginView { DataContext = loginViewModel };

            Application.Current.Windows.OfType<SignUpView>().FirstOrDefault()?.Close();
            loginView.ShowDialog(); 
        }

        private void SignUp()
        {
            using (var context = new AppDbContext())
            {
                var user = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    EmailAddress = EmailAddress,
                    UserName = UserName,
                    Password = PasswordHelper.HashPassword(Password),
                    UserType = Enumerations.UserType.Admin,

                };
                context.Users.Add(user);
                context.SaveChanges();

                SignUpStatus = "Sign Up Successful!";
            }
        }
   
    }
}
