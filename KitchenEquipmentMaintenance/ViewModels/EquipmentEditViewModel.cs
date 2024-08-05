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
using System.Windows;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class EquipmentEditViewModel :BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        private RegisteredEquipment _currentRegisteredEquipment;
        private Equipment equipment;
        private string _descriptionError;
        private string _serialNumberError;
        private bool _isValid;
        private bool _isFromRegisteredEquipment;
        private Site _parentSite;
        public Equipment Equipment
        { 
            get => equipment;
            set { equipment = value; OnPropertyChanged(nameof(Equipment)); }
        }
        private Site _selectedSite;
        public Site SelectedSite
        {
            get { return _selectedSite; }
            set
            {
                _selectedSite = value;
                OnPropertyChanged(nameof(SelectedSite));
            }
        }
        public Site ParentSite
        {
            get { return _parentSite; }
            set
            {
                _parentSite = value;
                OnPropertyChanged(nameof(ParentSite));
            }
        }
        public string DescriptionError
        {

            get { return _descriptionError; }
            set
            {
                _descriptionError = value;
                OnPropertyChanged(nameof(DescriptionError));
            }
        }
        public string SerialNumberError
        {

            get { return _serialNumberError; }
            set
            {
                _serialNumberError = value;
                OnPropertyChanged(nameof(SerialNumberError));
            }
        }
        public ObservableCollection<Site> AvailableSites { get; set; }

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
        public ICommand CancelCommand { get; }
        public EquipmentEditViewModel(Equipment equipment, Action<BaseViewModel> setCurrentViewModelAction, bool isFromRegisteredEquipment) 
        {
             _isFromRegisteredEquipment = isFromRegisteredEquipment;
              _setCurrentViewModelAction = setCurrentViewModelAction;
              Equipment = equipment;
              LoadSites();
              SelectedCondition = equipment.Condition;
              EditEquipmentCommand = new RelayCommand(Save);
              CancelCommand = new RelayCommand(CloseWindow);

        }
        private void LoadSites()
        {
            using (var context = new AppDbContext())
            {
                var registeredEquipment = context.RegisteredEquipments.Where(x=> x.EquipmentId == Equipment.EquipmentId).FirstOrDefault();
                SelectedSite = registeredEquipment?.Site;
                ParentSite = SelectedSite;

                AvailableSites = new ObservableCollection<Site>(context.Sites.Where(x => x.UserId == App.CurrentUser.UserId).ToList());
            }
        }
        protected void Save()
        {
            ValidateFields();
            if(_isValid)
            {
                using (var context = new AppDbContext())
                {
                    var existingEquipment = context.Equipments.Find(Equipment.EquipmentId);
                    var existingRegisteredEquipment = context.RegisteredEquipments.Where(x => x.EquipmentId == Equipment.EquipmentId).FirstOrDefault();

                    if (existingRegisteredEquipment != null)
                    {
                        existingRegisteredEquipment.SiteId = SelectedSite?.SiteId;
                    }

                    context.SaveChanges();

                    if (existingEquipment != null)
                    {
                        existingEquipment.SerialNumber = Equipment.SerialNumber;
                        existingEquipment.Description = Equipment.Description;
                        existingEquipment.Condition = SelectedCondition;

                        context.SaveChanges();
                    }

                    CloseWindow();
                }
            } 
        }
        private void ValidateFields()
        {
            _isValid = true;

            if (string.IsNullOrWhiteSpace(Equipment.Description))
            {
                DescriptionError = "Description is required";
                _isValid = false;
            }
            else
            {
                DescriptionError = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(Equipment.SerialNumber))
            {
                SerialNumberError = "Serial Number is required";
                _isValid = false;
            }
            else
            {
                SerialNumberError = string.Empty;
            }
        }
        private void CloseWindow()
        {
            if (_isFromRegisteredEquipment)
                _setCurrentViewModelAction?.Invoke(new RegisteredEquipmentViewModel(ParentSite, _setCurrentViewModelAction));
            else
                _setCurrentViewModelAction?.Invoke(new EquipmentMaintenanceViewModel(_setCurrentViewModelAction));
        }
    }
}
