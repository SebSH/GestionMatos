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
using WpfApplicationSlider.UserControls;
using System.Collections.ObjectModel;
using WpfApplicationSlider.Models;
using WpfApplicationSlider.ViewModels;
using WpfApplicationSlider.Services;

namespace WpfApplicationSlider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientViewModel _vm;
        private MaterielViewModel _vl;
        private SiteViewModel _vn;
        private InterventionViewModel _vp;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new ClientViewModel();
            this.DataContext = _vm;
            _vm.LoadClients();
            _vl = new MaterielViewModel();
            this.DataContext = _vl;
            _vl.LoadMateriels();
            _vn = new SiteViewModel();
            this.DataContext = _vn;
            _vn.LoadSites();
            _vp = new InterventionViewModel();
            this.DataContext = _vp;
            _vp.LoadInterventions();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void ClientClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ClientViewModel();

        }

        private void MaterielClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new MaterielViewModel();
        }

        private void SiteClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new SiteViewModel();
        }

        private void IntervClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new InterventionViewModel();
        }

       
    }
}
