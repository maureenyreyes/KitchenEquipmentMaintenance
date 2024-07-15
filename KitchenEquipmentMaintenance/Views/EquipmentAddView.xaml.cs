using KitchenEquipmentMaintenance.ViewModels;
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

namespace KitchenEquipmentMaintenance.Views
{
    /// <summary>
    /// Interaction logic for EquipmentAddView.xaml
    /// </summary>
    public partial class EquipmentAddView : Window
    {
        public EquipmentAddView()
        {
            InitializeComponent();
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is EquipmentAddViewModel viewModel)
            {
                viewModel.RaiseCanExecuteChanged();
            }
        }
    }
}
