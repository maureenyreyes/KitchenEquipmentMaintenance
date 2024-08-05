using KitchenEquipmentMaintenance.Views;
using System.Windows;
using System.Windows.Input;
using static KitchenEquipmentMaintenance.Models.Enumerations;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        private Window _adminView;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public ICommand UsersCommand { get; }
        public ICommand SiteCommand { get; }
        public ICommand EquipmentCommand { get; }
        public ICommand LogOutCommand { get; }

        public UserType CurrentUserType => App.CurrentUser.UserType;
        public string CurrentUserName => App.CurrentUser.UserName;

        public Visibility UsersMenuVisibility => CurrentUserType != UserType.SuperAdmin ? Visibility.Collapsed : Visibility.Visible;

        public AdminViewModel(Window adminView)
        {
            _adminView = adminView;

            UsersCommand = new RelayCommand(() => CurrentViewModel = new UserMaintenanceViewModel(SetCurrentViewModel));
            SiteCommand = new RelayCommand(() => CurrentViewModel = new SiteMaintenanceViewModel(SetCurrentViewModel));
            EquipmentCommand = new RelayCommand(() => CurrentViewModel = new EquipmentMaintenanceViewModel(SetCurrentViewModel));
            LogOutCommand = new RelayCommand(LogOut);

            if (CurrentUserType == UserType.SuperAdmin)
                CurrentViewModel = new UserMaintenanceViewModel(SetCurrentViewModel);
            else
                CurrentViewModel = new SiteMaintenanceViewModel(SetCurrentViewModel);
        }

        private void SetCurrentViewModel(BaseViewModel viewModel)
        {
            CurrentViewModel = viewModel;
        }

        protected void LogOut()
        {
            var loginView = new LoginView();
            loginView.Show();
            _adminView.Close();
        }
    }
}
