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
        public Site _site;

        public Site Site
        {
            get { return _site; }
            set
            {
                _site = value;
                OnPropertyChanged(nameof(Site));
            }
        }

        public ICommand EditSiteCommand { get;}

        public SiteEditViewModel(Site site)
        {
            Site = site;
            EditSiteCommand = new RelayCommand(Save);
        }

        private void Save()
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
            }
            // Close the window after saving
            Application.Current.Windows.OfType<SiteEditView>().FirstOrDefault()?.Close();
        }
    }
}
