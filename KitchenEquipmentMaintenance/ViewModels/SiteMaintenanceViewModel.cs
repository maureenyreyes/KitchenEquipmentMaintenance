using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KitchenEquipmentMaintenance.ViewModels
{
    public class SiteMaintenanceViewModel : BaseViewModel
    {
        private readonly Action<BaseViewModel> _setCurrentViewModelAction;

        public ObservableCollection<Site> _sites;
        public ObservableCollection<Site> Sites
        {
            get => _sites;
            set { _sites = value; OnPropertyChanged(); }
        }

        public Site _selectedSite;
        public Site SelectedSite
        {
            get => _selectedSite;
            set { _selectedSite = value; OnPropertyChanged(); }
        }

        public ICommand AddSiteCommand { get; }
        public ICommand EditSiteCommand { get; }
        public ICommand DeleteSiteCommand { get; }
        public ICommand ViewEquipmentCommand { get; }

        public SiteMaintenanceViewModel(Action<BaseViewModel> setCurrentViewModelAction)
        {
            _setCurrentViewModelAction = setCurrentViewModelAction;
            LoadSites();

            AddSiteCommand = new RelayCommand(AddSite);
            EditSiteCommand = new RelayCommand<Site>(EditSite);
            DeleteSiteCommand = new RelayCommand<Site>(DeleteSite);
            ViewEquipmentCommand = new RelayCommand(ViewEquipment);
        }

        private void ViewEquipment()
        {
            if (SelectedSite != null)
            {
                _setCurrentViewModelAction?.Invoke(new RegisteredEquipmentViewModel(SelectedSite, _setCurrentViewModelAction));
            }

        }

        private void AddSite()
        {
            _setCurrentViewModelAction?.Invoke(new SiteAddViewModel(_setCurrentViewModelAction));

            LoadSites();
        }
        private void EditSite(Site site) 
        {
            _setCurrentViewModelAction?.Invoke(new SiteEditViewModel(site, _setCurrentViewModelAction));          
            LoadSites();
        }

        private void DeleteSite(Site site)
        {
            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingSite = context.Sites.Find(site.SiteId);
                        if (existingSite == null)
                        {
                            throw new InvalidOperationException("Site not found in the database.");
                        }

                        var existingRegisteredEquipment = context.RegisteredEquipments
                            .Where(x => x.SiteId == site.SiteId).ToList();

                        foreach (var item in existingRegisteredEquipment)
                        {
                            item.SiteId = null;
                        } 

                        context.SaveChanges();

                        context.Sites.Remove(existingSite);
                        context.SaveChanges();

                        Sites.Remove(site);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            LoadSites();
        }

        private void LoadSites()
        {
            using (var context = new AppDbContext())
            {
                _sites = new ObservableCollection<Site>(context.Sites.Where(x => x.UserId == App.CurrentUser.UserId).ToList());
            }
            OnPropertyChanged(nameof(Sites));
        }
        private bool CanEditOrDelete()
        {
            return SelectedSite != null;
        }
    }
}
