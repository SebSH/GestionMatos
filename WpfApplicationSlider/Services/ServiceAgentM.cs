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
    
    public class ServiceAgentM
    {
        public static void GetMateriels(Action<ObservableCollection<Materiel>, Exception> completed)
        {
            try
            {
                ObservableCollection<Materiel> materiels = Materiels.GetMateriel();
                completed(materiels, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void GetFilteredMateriel(Action<ObservableCollection<Materiel>, Exception> completed, int SelectedClient, int SelectedSite)
        {
            try
            {
                ObservableCollection<Materiel> filteredmateriels = Materiels.GetFilteredMateriel(SelectedClient, SelectedSite);
                completed(filteredmateriels, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void Flush(ObservableCollection<Materiel> list, Action<Exception> completed)
        {
            try
            {
                Materiels.Flush(list);
                completed(null);
            }
            catch (Exception e)
            {
                completed(e);
            }
        }

    }
}
