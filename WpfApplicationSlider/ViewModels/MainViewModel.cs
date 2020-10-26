using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplicationSlider.Models;

namespace WpfApplicationSlider.ViewModels
{
    class MainViewModel
    {
        private static MainViewModel _Instance;
        public static MainViewModel GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new MainViewModel();
            }
            return _Instance;
        }
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                

            }
        }

        private ObservableCollection<Site> sites;
        public ObservableCollection<Site> Sites
        {
            get { return sites; }
            set
            {
                sites = value;
                

            }
        }

        private ObservableCollection<Materiel> materiels;
        public ObservableCollection<Materiel> Materiels
        {
            get { return materiels; }
            set
            {
                materiels = value;


            }
        }

        private ObservableCollection<Interv> intervs;
        public ObservableCollection<Interv> Intervs
        {
            get { return intervs; }
            set
            {
                intervs = value;


            }
        }
    }
}
