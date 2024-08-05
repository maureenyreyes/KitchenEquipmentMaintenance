using KitchenEquipmentMaintenance.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using KitchenEquipmentMaintenance.Views;
using KitchenEquipmentMaintenance.Helpers;
using System.Collections.Generic;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        #region Private Fields
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _userName;
        private string _password;
        private string _signUpStatus;
        private bool _isValid;

        private string _usernameError;
        private string _passwordError;
        private string _firstNameError;
        private string _lastNameError;
        private string _emailAddressError;
        #endregion

        #region Public Properties
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged();}
        }
        public string EmailAddress
        {
            get => _emailAddress;
            set { _emailAddress = value; OnPropertyChanged(); }
        }
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged();  }
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

        public string UsernameError
        {
            get => _usernameError;
            set { _usernameError = value; OnPropertyChanged(); }
        }
        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(); }
        }
        public string FirstNameError
        {
            get => _firstNameError;
            set { _firstNameError = value; OnPropertyChanged(); }
        }
        public string LastNameError
        {
            get => _lastNameError;
            set { _lastNameError = value; OnPropertyChanged(); }
        }
        public string EmailAddressError
        {
            get => _emailAddressError;
            set { _emailAddressError = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand SignUpCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructor
        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(SignUp);
            CancelCommand = new RelayCommand(() => BackToLogin(string.Empty));
        }
        #endregion

        #region Private Methods
        private void BackToLogin(string signUpStatus)
        {
            var loginViewModel = new LoginViewModel { SignUpStatus = signUpStatus };
            var loginView = new LoginView { DataContext = loginViewModel };

            Application.Current.Windows.OfType<SignUpView>().FirstOrDefault()?.Close();
            loginView.ShowDialog();
        }

        private void SignUp()
        {
            OnPropertyChanged(string.Empty);
            ValidateFields();

            if (_isValid)
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
                    BackToLogin(SignUpStatus);
                }
            }
            else
            {
                SignUpStatus = "Sign Up Failed! Please correct the errors and try again.";
            }
        }

        private void ValidateFields()
        {
            _isValid = true;

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameError = "First Name is required";
                _isValid = false;
            }
            else
            {
                FirstNameError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameError = "Last Name is required";
                _isValid = false;
            }
            else
            {
                LastNameError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                EmailAddressError = "Email Address is required";
                _isValid = false;
            }
            else
            {
                EmailAddressError = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                UsernameError = "Username is required";
                _isValid = false;
            }
            else
            {
                using (var context = new AppDbContext())
                {
                    var existingUser = context.Users.FirstOrDefault(x => x.UserName == UserName);
                    if (existingUser != null)
                    {
                        UsernameError = "Username is already in use";
                        _isValid = false;
                    }
                    else
                    {
                        UsernameError = string.Empty;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                _isValid = false;
            }
            else
            {
                PasswordError = string.Empty;
            }
        }
        #endregion
    }
}
