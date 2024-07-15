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

        public event Action<BaseViewModel> RequestViewEquipment;

        private void ViewEquipment()
        {
            //RequestViewEquipment?.Invoke(new RegisteredEquipmentViewModel(SelectedSite));

            //var registeredEquipmentViewModel = new RegisteredEquipmentViewModel(SelectedSite);
            //var registeredEquipmentView = new RegisteredEquipmentView { DataContext = registeredEquipmentViewModel };

            //registeredEquipmentView.ShowDialog();
            if (SelectedSite != null)
            {
                _setCurrentViewModelAction?.Invoke(new RegisteredEquipmentViewModel(SelectedSite));
            }

        }

        private void AddSite()
        {
            var addSiteViewModel = new SiteAddViewModel();
            var addSiteView = new SiteAddView { DataContext = addSiteViewModel };

            addSiteView.ShowDialog();

            LoadSites();
        }
        private void EditSite(Site site) 
        {
            var editSiteViewModel = new SiteEditViewModel(site);
            var editSiteView = new SiteEditView { DataContext = editSiteViewModel };
            editSiteView.ShowDialog();
            LoadSites();
        }
        private void DeleteSite(Site site)
        {
            using (var context = new AppDbContext())
            {
                // Check if the user is already attached to the context
                var existingSite = context.Sites.Find(site.SiteId);

                if (existingSite != null)
                {
                    context.Sites.Remove(existingSite); // Remove the attached entity
                    context.SaveChanges();
                    Sites.Remove(site); // Remove from the local collection
                }
                else
                {
                    throw new InvalidOperationException("Site not found in the database.");
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
