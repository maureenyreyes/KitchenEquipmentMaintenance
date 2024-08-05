using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class RegisteredEquipmentViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        private readonly AppDbContext _context;
        private bool _isFromRegisteredEquipment = true;
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

        public RegisteredEquipmentViewModel(Site selectedSite, Action<BaseViewModel> setCurrentViewModelAction)
        {
            _setCurrentViewModelAction = setCurrentViewModelAction;
            _context = new AppDbContext();
            SelectedSite = selectedSite;
            LoadEquiments();

            AddEquipmentCommand = new RelayCommand(AddEquipment);
            EditEquipmentCommand = new RelayCommand<Equipment>(EditEquipment);
            DeleteEquipmentCommand = new RelayCommand<Equipment>(DeleteEquipment);
        }

        private void LoadEquiments()
        {
            var equipmentsWithRegisteredEquipments = _context.Equipments.Where(x => x.UserId == App.CurrentUser.UserId && x.RegisteredSites.Any(rs => rs.SiteId == SelectedSite.SiteId))
                    .Include(e => e.RegisteredSites)
                    .ToList();

            Equipments = new ObservableCollection<Equipment>(equipmentsWithRegisteredEquipments);

            OnPropertyChanged(nameof(Equipments));
        }

        private void AddEquipment()
        {
            //var equipmentAddViewModel = new EquipmentAddViewModel(SelectedSite);
            //var equipmentAddView = new EquipmentAddView { DataContext = equipmentAddViewModel };
            //equipmentAddView.ShowDialog();

            _setCurrentViewModelAction?.Invoke(new EquipmentAddViewModel(SelectedSite, _setCurrentViewModelAction, _isFromRegisteredEquipment));
            LoadEquiments();
        }

        private void EditEquipment(Equipment equipment)
        {
            //var equipmentEditViewModel = new EquipmentEditViewModel(equipment);
            //var equipmentEditView = new EquipmentEditView { DataContext = equipmentEditViewModel };
            //equipmentEditView.ShowDialog();
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

