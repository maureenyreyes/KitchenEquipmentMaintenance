using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class EquipmentAddViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        private Equipment equipment;
        private string _descriptionError;
        private string _serialNumberError;
        private bool _isValid;
        private bool _isFromRegisteredEquipment;
        private Site _selectedSite;
        private ObservableCollection<Site> _availableSites;

        public Site SelectedSite
        {
            get => _selectedSite;
            set
            {
                if (_selectedSite != value)
                {
                    _selectedSite = value;
                    OnPropertyChanged(nameof(SelectedSite));
                    OnPropertyChanged(nameof(IsComboBoxEnabled));
                }
            }
        }

        public ObservableCollection<Site> AvailableSites
        {
            get => _availableSites;
            set
            {
                if (_availableSites != value)
                {
                    _availableSites = value;
                    OnPropertyChanged(nameof(AvailableSites));
                }
            }
        }

        public Equipment Equipment 
        { 
            get => equipment;
            set { equipment = value; OnPropertyChanged(nameof(Equipment)); }
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
        public bool IsComboBoxEnabled => SelectedSite == null;
        public ICommand AddNewEquipmentCommand { get; }
        public ICommand CancelCommand { get; }

        public EquipmentAddViewModel(Site selectedSite, Action<BaseViewModel> setCurrentViewModelAction, bool isFromRegisteredEquipment) 
        {
              _isFromRegisteredEquipment = isFromRegisteredEquipment;
             _setCurrentViewModelAction = setCurrentViewModelAction;
              LoadSites();

                if (selectedSite != null)
                {
                    SelectedSite = AvailableSites.FirstOrDefault(site => site.SiteId == selectedSite.SiteId);
                }

              Equipment = new Equipment();
              AddNewEquipmentCommand = new RelayCommand(Save);
              CancelCommand = new RelayCommand(CloseWindow);
            SelectedCondition = "Working";


        }

        private void LoadSites()
        {
            using (var context = new AppDbContext())
            {
                AvailableSites = new ObservableCollection<Site>(context.Sites.Where(x => x.UserId == App.CurrentUser.UserId).ToList());
            }
        }

        private void Save()
        {
            ValidateFields();
            if (_isValid)
            {
                using (var context = new AppDbContext())
                {
                    Equipment.UserId = App.CurrentUser.UserId;
                    Equipment.Condition = SelectedCondition;

                    context.Equipments.Add(Equipment);
                    context.SaveChanges();

                    var registeredEquipment = new RegisteredEquipment
                    {
                        EquipmentId = Equipment.EquipmentId,
                        SiteId = SelectedSite?.SiteId 
                    };

                    context.RegisteredEquipments.Add(registeredEquipment);
                    context.SaveChanges();
                }
                CloseWindow();
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
            if(_isFromRegisteredEquipment)
                _setCurrentViewModelAction?.Invoke(new RegisteredEquipmentViewModel(SelectedSite, _setCurrentViewModelAction));  
            else
                _setCurrentViewModelAction?.Invoke(new EquipmentMaintenanceViewModel(_setCurrentViewModelAction));
        }
    }
}
