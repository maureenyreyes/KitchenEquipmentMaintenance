using KitchenEquipmentMaintenance.Helpers;
using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private string _userName;
        private string _password;
        private string _loginStatus;
        private string _signUpStatus;
        private string _passwordError;
        private string _usernameError;
        private bool _isPasswordValid;
        private bool _isUsernameValid;

        #endregion

        #region Properties

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                if (_passwordError != value)
                {
                    _passwordError = value;
                    OnPropertyChanged();
                }
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

        public string LoginStatus
        {
            get => _loginStatus;
            set
            {
                _loginStatus = value;
                OnPropertyChanged();
            }
        }

        public string SignUpStatus
        {
            get => _signUpStatus;
            set
            {
                _signUpStatus = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignUpStatusVisibility));
            }
        }

        public Visibility SignUpStatusVisibility => string.IsNullOrEmpty(SignUpStatus) ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }
        public ICommand SignUpCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            SignUpCommand = new RelayCommand(SignUp);
        }

        #endregion

        #region Methods

        private void Login()
        {
            ValidateCredentials();

            if (_isPasswordValid && _isUsernameValid)
            {
                using (var context = new AppDbContext())
                {
                    var hashedPassword = PasswordHelper.HashPassword(Password);
                    var user = context.Users.FirstOrDefault(u => u.UserName == UserName && u.Password == hashedPassword);

                    if (user != null)
                    {
                        StoreCurrentUser(user);
                        ShowAdminView();
                    }
                    else
                    {
                        LoginStatus = "Invalid Username or Password!";
                    }
                }
            }
        }

        private void StoreCurrentUser(User user)
        {
            App.CurrentUser = user;
        }

        private void ShowAdminView()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var adminView = new AdminView();
                adminView.Show();
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView)?.Close();
            });
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

        private void ValidateCredentials()
        {
            _isPasswordValid = !string.IsNullOrWhiteSpace(Password);
            PasswordError = _isPasswordValid ? string.Empty : "Password is required";

            _isUsernameValid = !string.IsNullOrWhiteSpace(UserName);
            UsernameError = _isUsernameValid ? string.Empty : "Username is required";

            SignUpStatus = string.Empty;
        }

        #endregion
    }
}
