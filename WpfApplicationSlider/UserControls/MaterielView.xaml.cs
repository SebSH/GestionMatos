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
using WpfApplicationSlider.ViewModels;
using WpfApplicationSlider.Models;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using WpfApplicationSlider.View;
using WpfApplicationSlider.Services;

namespace WpfApplicationSlider.UserControls
{
    /// <summary>
    /// Interaction logic for MaterielView.xaml
    /// </summary>
    public partial class MaterielView : UserControl
    {
        private MaterielViewModel _vm;
        public MaterielView()
        {
            InitializeComponent();
            _vm = new MaterielViewModel();
            this.DataContext = _vm;
            _vm.LoadMateriels();
        }
    }
}
