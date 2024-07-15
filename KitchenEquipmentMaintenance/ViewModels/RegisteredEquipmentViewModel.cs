using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class RegisteredEquipmentViewModel : BaseViewModel
    {
        private ObservableCollection<Equipment> _equipments;
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value; OnPropertyChanged(); }

        }
        private string selectedCondition;
        public string SelectedCondition
        {
            get => selectedCondition;
            set
            {
                selectedCondition = value;
                if (SelectedEquipment != null)
                {
                    SelectedEquipment.Condition = selectedCondition;
                }
                OnPropertyChanged();
            }
        }

        private Equipment _selectedEquipment;
        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set { _selectedEquipment = value; OnPropertyChanged(); SelectedCondition = _selectedEquipment?.Condition; }

        }
        private Site _selectedSite;
        public Site SelectedSite
        {
            get => _selectedSite;
            set { _selectedSite = value; OnPropertyChanged(); }

        }

        public ICommand AddEquipmentCommand { get; }
        public ICommand EditEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }

        public RegisteredEquipmentViewModel(Site selectedSite)
        {
            SelectedSite = selectedSite;
            LoadEquiments();

            AddEquipmentCommand = new RelayCommand(AddEquipment);
            EditEquipmentCommand = new RelayCommand<Equipment>(EditEquipment);
            DeleteEquipmentCommand = new RelayCommand<Equipment>(DeleteEquipment);
        }

        private void LoadEquiments()
        {
            using (var context = new AppDbContext())
            {
                Equipments = new ObservableCollection<Equipment>(context.Equipments.Where(x => x.UserId == App.CurrentUser.UserId && x.RegisteredSites.Any(rs => rs.SiteId == SelectedSite.SiteId)).ToList());
            }
            OnPropertyChanged(nameof(Equipments));
        }

        private void AddEquipment()
        {
            var equipmentAddViewModel = new EquipmentAddViewModel(SelectedSite);
            var equipmentAddView = new EquipmentAddView { DataContext = equipmentAddViewModel };
            equipmentAddView.ShowDialog();

            LoadEquiments();
        }

        private void EditEquipment(Equipment equipment)
        {
            var equipmentEditViewModel = new EquipmentEditViewModel(equipment);
            var equipmentEditView = new EquipmentEditView { DataContext = equipmentEditViewModel };
            equipmentEditView.ShowDialog();

            LoadEquiments();
        }
        private void DeleteEquipment(Equipment equipment)
        {
            using (var context = new AppDbContext())
            {
                var existingEquipment = context.Equipments.Find(equipment.EquipmentId);

                if (existingEquipment != null)
                {
                    context.Equipments.Remove(existingEquipment);
                    context.SaveChanges();
                    Equipments.Remove(equipment);
                }
                else
                {
                    throw new InvalidOperationException("Equipment not found in the database.");
                }
            }
        }
    }
}

