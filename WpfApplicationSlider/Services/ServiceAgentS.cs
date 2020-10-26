using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;
using WpfApplicationSlider.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;




namespace WpfApplicationSlider.Services
{
   
    public static class ServiceAgentS 
    {
        public static void GetSites(Action<ObservableCollection<Site>, Exception> completed)
        {
            try
            {
                ObservableCollection<Site> sites = Sites.GetSite();
                completed(sites, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void GetFilteredSites(Action<ObservableCollection<Site>, Exception> completed, int SelectedClient)
        {
            try
            {
                ObservableCollection<Site> filteredsites = Sites.GetFilteredSite(SelectedClient);
                completed(filteredsites, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void Flush(ObservableCollection<Site> list, Action<Exception> completed)
        {
            try
            {
                Sites.Flush(list);
                completed(null);
            }
            catch (Exception e)
            {
                completed(e);
            }
        }

    }
}