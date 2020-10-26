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


    public static class ServiceAgent 
    {
        public static void GetClients(Action<ObservableCollection<Client>, Exception> completed)
        {
            try
            {
                ObservableCollection<Client> clients = Clients.GetClient();
                completed(clients, null);
            }
            catch (Exception e)
            {
                completed(null, e);
            }
        }

        public static void Flush(ObservableCollection<Client> list, Action<Exception> completed)
        {
            try
            {
                Clients.Flush(list);
                completed(null);
            }
            catch (Exception e)
            {
                completed(e);
            }
        }

    }
}
    

