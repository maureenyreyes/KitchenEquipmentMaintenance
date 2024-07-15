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
    public class SiteAddViewModel : BaseViewModel
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
        public ICommand AddNewSiteCommand { get; }

        public SiteAddViewModel() 
        {

            Site = new Site();
            AddNewSiteCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            using (var context = new AppDbContext())
            {
                Site.UserId = App.CurrentUser.UserId;
                context.Sites.Add(Site);
                context.SaveChanges();

                // Close the window after saving
                Application.Current.Windows.OfType<SiteAddView>().FirstOrDefault()?.Close();
            }

        }
    }
}
