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
using System.Windows.Shapes;
using KitchenEquipmentMaintenance.Models;
using KitchenEquipmentMaintenance.ViewModels;

namespace KitchenEquipmentMaintenance.Views
{
    /// <summary>
    /// Interaction logic for UserMaintenanceView.xaml
    /// </summary>
    public partial class UserMaintenanceView : UserControl
    {
        public UserMaintenanceView()
        {
            InitializeComponent();
            DataContext = new UserMaintenanceViewModel();
        }

       
    }
}
