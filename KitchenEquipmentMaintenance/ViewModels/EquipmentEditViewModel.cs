using KitchenEquipmentMaintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using KitchenEquipmentMaintenance.Views;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class EquipmentEditViewModel :BaseViewModel
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
                Equipment.Condition = value;
                OnPropertyChanged(nameof(SelectedCondition));
            }
        }
        public ICommand EditEquipmentCommand { get; }
        public EquipmentEditViewModel(Equipment equipment) 
        {
          Equipment = equipment;
          SelectedCondition = equipment.Condition;
          EditEquipmentCommand = new RelayCommand(Save);

        }
        protected void Save()
        {
            using (var context = new AppDbContext())
            {
                var existingEquipment = context.Equipments.Find(Equipment.EquipmentId);
                if (existingEquipment != null)
                {
                    existingEquipment.SerialNumber = Equipment.SerialNumber;
                    existingEquipment.Description = Equipment.Description;
                    existingEquipment.Condition = SelectedCondition;

                    context.SaveChanges();
                }

                Application.Current.Windows.OfType<EquipmentEditView>().FirstOrDefault()?.Close();
            }
        }
    }
}
