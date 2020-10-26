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
    class ServiceAgentI
    {
        public static void GetInterventions(Action<ObservableCollection<Interv>, Exception> completed)
        {
            try
            {
                ObservableCollection<Interv> intervs = Interventions.GetInterv();
                completed(intervs, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void Flush(ObservableCollection<Interv> list, Action<Exception> completed)
        {
            try
            {
                Interventions.Flush(list);
                completed(null);
            }
            catch (Exception e)
            {
                completed(e);
            }
        }
    }
}
