using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views.User;
using System.Windows;
using KitchenEquipmentMaintenance.Views;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
    }
}
