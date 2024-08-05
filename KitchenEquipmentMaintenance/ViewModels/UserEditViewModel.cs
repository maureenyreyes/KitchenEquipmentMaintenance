using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using static KitchenEquipmentMaintenance.Models.Enumerations;
using System.Net.Mail;
using KitchenEquipmentMaintenance.Helpers;


namespace KitchenEquipmentMaintenance.ViewModels
{
    public class UserEditViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        private User _user;
        private string _usernameError;
        private string _firstNameError;
        private string _lastNameError;
        private string _emailAddressError;
        private bool _isValid;
        private string _selectedUserType;
        public string SelectedUserType
        {
            get => _selectedUserType;
            set
            {
                _selectedUserType = value;
                User.UserType = (UserType)Enum.Parse(typeof(UserType), value);
                OnPropertyChanged();
            }
        }
    
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public string UsernameError
        {
            get => _usernameError;
            set
            {
                if (_usernameError != value)
                {
                    _usernameError = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FirstNameError
        {
            get => _firstNameError;
            set
            {
                if (_firstNameError != value)
                {
                    _firstNameError = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LastNameError
        {
            get => _lastNameError;
            set
            {
                if (_lastNameError != value)
                {
                    _lastNameError = value;
                    OnPropertyChanged();
                }
            }
        }
        public string EmailAddressError
        {
            get => _emailAddressError;
            set
            {
                if (_emailAddressError != value)
                {
                    _emailAddressError = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserEditViewModel(User user, Action<BaseViewModel> setCurrentViewModelAction)
        {
            _setCurrentViewModelAction = setCurrentViewModelAction;
            User = user;
            SelectedUserType = user.UserType.ToString();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(CloseWindow);
        }    
        private void Save()
        {   
            ValidateFields();

            if(_isValid)
            {
                using (var context = new AppDbContext())
                {
                    var existingUser = context.Users.Find(User.UserId);
                    if (existingUser != null)
                    {
                        existingUser.UserType = User.UserType;
                        existingUser.FirstName = User.FirstName;
                        existingUser.LastName = User.LastName;
                        existingUser.EmailAddress = User.EmailAddress;
                        existingUser.UserName = User.UserName;
                        context.Entry(existingUser).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                }
                CloseWindow();
            }
        }
        private void ValidateFields()
        {
            _isValid = true;

            if (string.IsNullOrWhiteSpace(User.UserName))
            {
                UsernameError = "User Name is required";
                _isValid = false;
            }
            else
            {
                UsernameError = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(User.FirstName))
            {
                FirstNameError = "First Name is required";
                _isValid = false;
            }
            else
            {
                FirstNameError = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(User.LastName))
            {
                LastNameError = "Last Name is required";
                _isValid = false;
            }
            else
            {
                LastNameError = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(User.EmailAddress))
            {
                EmailAddressError = "Last Name is required";
                _isValid = false;
            }
            else
            {
                EmailAddressError = string.Empty;
            }
        }
        private void CloseWindow()
        {
            _setCurrentViewModelAction?.Invoke(new UserMaintenanceViewModel(_setCurrentViewModelAction));
        }
    }
}
