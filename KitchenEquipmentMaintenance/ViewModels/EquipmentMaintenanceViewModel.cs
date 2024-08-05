using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity; 

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class EquipmentMaintenanceViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        private readonly AppDbContext _context;
        private bool _isFromRegisteredEquipment = false;
        public ObservableCollection<Site> AvailableSites { get; set; }
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

        public ICommand AddEquipmentCommand { get; }
        public ICommand EditEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }

        public EquipmentMaintenanceViewModel(Action<BaseViewModel> setCurrentViewModelAction)
        {
            _setCurrentViewModelAction = setCurrentViewModelAction;
            _context = new AppDbContext();

            LoadEquiments();
            LoadSites();

            AddEquipmentCommand = new RelayCommand(AddEquipment);
            EditEquipmentCommand = new RelayCommand<Equipment>(EditEquipment);
            DeleteEquipmentCommand = new RelayCommand<Equipment>(DeleteEquipment);
        }

        private void LoadEquiments()
        {
            var equipmentsWithRegisteredEquipments = _context.Equipments.Where(x => x.UserId == App.CurrentUser.UserId)
                    .Include(e => e.RegisteredSites)
                    .ToList();

            Equipments = new ObservableCollection<Equipment>(equipmentsWithRegisteredEquipments);

            OnPropertyChanged(nameof(Equipments));
        }
        private void LoadSites()
        {
            using (var context = new AppDbContext())
            {
                AvailableSites = new ObservableCollection<Site>(context.Sites.Where(x => x.UserId == App.CurrentUser.UserId).ToList());
            }
        }

        private void AddEquipment()
        {           
            _setCurrentViewModelAction?.Invoke(new EquipmentAddViewModel(null, _setCurrentViewModelAction, _isFromRegisteredEquipment));

            LoadEquiments();
        }

        private void EditEquipment(Equipment equipment)
        {         
            _setCurrentViewModelAction?.Invoke(new EquipmentEditViewModel(equipment, _setCurrentViewModelAction, _isFromRegisteredEquipment));
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
