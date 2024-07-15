using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class EquipmentAddViewModel : BaseViewModel, IDataErrorInfo
    {
        private Equipment equipment;
        public Equipment Equipment 
        { 
            get => equipment;
            set { equipment = value; OnPropertyChanged(nameof(Equipment)); }
        }
        private string selectedCondition;
        public string SelectedCondition
        {
            get => selectedCondition;
            set
            {
                selectedCondition = value;
                OnPropertyChanged();
            }
        }
        public Site _selectedSite;
        public Site SelectedSite
        {
            get => _selectedSite;
            set { _selectedSite = value; OnPropertyChanged(); }
        }
        public ICommand AddNewEquipmentCommand { get; }

        public EquipmentAddViewModel(Site selectedSite) 
        { 
              SelectedSite = selectedSite;
              Equipment = new Equipment();
              AddNewEquipmentCommand = new RelayCommand(Save, CanAddNewEquipment);
        }

        private void Save()
        {
            using (var context = new AppDbContext())
            {
                Equipment.UserId = App.CurrentUser.UserId;
                Equipment.Condition = SelectedCondition;
                context.Equipments.Add(Equipment);
                context.SaveChanges();

                if (SelectedSite != null)
                {
                    var registeredEquipment = new RegisteredEquipment
                    {
                        EquipmentId = Equipment.EquipmentId,
                        SiteId = SelectedSite.SiteId,
                    };

                    context.RegisteredEquipments.Add(registeredEquipment);
                    context.SaveChanges();
                }
            }
            Application.Current.Windows.OfType<EquipmentAddView>().FirstOrDefault()?.Close();   
        }


        private bool CanAddNewEquipment()
        {
            return string.IsNullOrWhiteSpace(this[nameof(Equipment.SerialNumber)]) &&
                   string.IsNullOrWhiteSpace(this[nameof(Equipment.Description)]) &&
                   string.IsNullOrWhiteSpace(this[nameof(SelectedCondition)]);
        }

        #region IDataErrorInfo Implementation

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Equipment.SerialNumber):
                        if (string.IsNullOrWhiteSpace(Equipment.SerialNumber))
                        {
                            error = "SerialNumber cannot be empty.";
                        }
                        break;
                    case nameof(Equipment.Description):
                        if (string.IsNullOrWhiteSpace(Equipment.Description))
                        {
                            error = "Description cannot be empty.";
                        }
                        break;
                    case nameof(SelectedCondition):
                        if (string.IsNullOrWhiteSpace(SelectedCondition))
                        {
                            error = "Condition cannot be empty.";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        #endregion

        public void RaiseCanExecuteChanged()
        {
            (AddNewEquipmentCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
