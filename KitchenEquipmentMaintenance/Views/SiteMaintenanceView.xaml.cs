using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KitchenEquipmentMaintenance.ViewModels;

namespace KitchenEquipmentMaintenance.Views
{
    /// <summary>
    /// Interaction logic for SiteMaintenanceView.xaml
    /// </summary>
    public partial class SiteMaintenanceView : UserControl
    {
        public SiteMaintenanceView()
        {
            InitializeComponent();
            //DataContext = new SiteMaintenanceViewModel();
        }
    }
}
