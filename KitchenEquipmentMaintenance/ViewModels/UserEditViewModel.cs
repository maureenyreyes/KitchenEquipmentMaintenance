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


namespace KitchenEquipmentMaintenance.ViewModels
{
    public class UserEditViewModel : BaseViewModel
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        public UserEditViewModel(User user)
        {
            User = user;
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
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
                    existingUser.Password = User.Password; // Handle password securely
                    context.Entry(existingUser).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
            // Close the window after saving
            Application.Current.Windows.OfType<UserEditView>().FirstOrDefault()?.Close();
        }
    }
}
