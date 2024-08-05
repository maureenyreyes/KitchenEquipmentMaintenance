using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views.User;
using System.Windows;
using KitchenEquipmentMaintenance.Views;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class SiteEditViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;
        public Site _site;
        private string _descriptionError;
        private bool _isValid;

        public Site Site
        {
            get { return _site; }
            set
            {
                _site = value;
                OnPropertyChanged(nameof(Site));
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

        public ICommand EditSiteCommand { get;}
        public ICommand CancelCommand { get;}

        public SiteEditViewModel(Site site, Action<BaseViewModel> setCurrentViewModelAction)
        {
            _setCurrentViewModelAction = setCurrentViewModelAction;
            Site = site;
            EditSiteCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(CloseWindow);  
        }

        private void Save()
        {
            ValidateFields();
            if (_isValid)
            {
               using (var context = new AppDbContext())
                {
                    var existingSite = context.Sites.Find(Site.SiteId);
                    if (existingSite != null)
                    {
                        existingSite.SiteId = Site.SiteId;
                        existingSite.Description = Site.Description;
                        existingSite.Active = Site.Active;

                    }
                    context.SaveChanges();
                    CloseWindow();

                }
            }
         
        }
        private void ValidateFields()
        {
            _isValid = true;

            if (string.IsNullOrWhiteSpace(Site.Description))
            {
                DescriptionError = "Description is required";
                _isValid = false;
            }
            else
            {
                DescriptionError = string.Empty;
            }
        }
        private void CloseWindow()
        {
            _setCurrentViewModelAction?.Invoke(new SiteMaintenanceViewModel(_setCurrentViewModelAction));
        }
    }
}
