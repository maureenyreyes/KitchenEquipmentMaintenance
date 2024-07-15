using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class UserMaintenanceViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }
        public ICommand DeleteUserCommand { get; }
        public ICommand EditCommand { get; }
        public UserMaintenanceViewModel() 
        {
            using (var context = new AppDbContext())
            {
                Users = new ObservableCollection<User>(context.Users.ToList());
            }

            DeleteUserCommand = new RelayCommand<User>(DeleteUser);
            EditCommand = new RelayCommand<User>(EditUser);
        }

        private void DeleteUser(User user)
        {
            using (var context = new AppDbContext())
            {
                // Check if the user is already attached to the context
                var existingUser = context.Users.Find(user.UserId);

                if (existingUser != null)
                {
                    context.Users.Remove(existingUser); // Remove the attached entity
                    context.SaveChanges();
                    Users.Remove(user); // Remove from the local collection
                }
                else
                {                  
                    throw new InvalidOperationException("User not found in the database.");
                }
            }
        }

        private void EditUser(User user)
        {
            if (user == null) return;
            var editUserViewModel = new UserEditViewModel(user);
            var editUserView = new UserEditView { DataContext = editUserViewModel };
            editUserView.ShowDialog();

            // Refresh Users collection after editing
            using (var context = new AppDbContext())
            {
                var users = context.Users.ToList();
                Users.Clear();
                foreach (var u in users)
                {
                    Users.Add(u);
                }
            }
        }
    }
}
