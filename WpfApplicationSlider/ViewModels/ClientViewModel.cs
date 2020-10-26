using System;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SimpleMvvmToolkit;
using WpfApplicationSlider.Models;
using WpfApplicationSlider.Services;
using System.Windows.Controls;



namespace WpfApplicationSlider.ViewModels
{
    class ClientViewModel: ViewModelBase<ClientViewModel>
    {
      public ClientViewModel() { }

    
        #region Notifications

        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        #endregion

        #region Properties

      
        private string nomClient;
        public string NomClient
        {
            get { return nomClient; }
            set
            {
                nomClient = value;
                NotifyPropertyChanged(m => m.NomClient);
            }
        }

    

        private string adresse;
        public string Adresse
        {
            get { return adresse; }
            set
            {
                adresse = value;
                NotifyPropertyChanged(m => m.Adresse);
            }
        }

        private int? cp;
        public int? CP
        {
            get { return cp; }
            set
            {
                cp = value;
                NotifyPropertyChanged(m => m.CP);
            }
        }

        private int? telephone;
        public int? Telephone
        {
            get { return telephone; }
            set
            {
                telephone = value;
                NotifyPropertyChanged(m => m.Telephone);
            }
        }
        
        public ObservableCollection<Client> Clients
        {
            get { return MainViewModel.GetInstance().Clients; }
            set
            {
               if((value != null) && (value != MainViewModel.GetInstance().Clients))
               {
                   MainViewModel.GetInstance().Clients = value;
                   NotifyPropertyChanged(m => m.Clients);
               }
            }
        }

        private ObservableCollection<Client> _Filteredclients;
        public ObservableCollection<Client> Filteredclients
        {
            get
            {

                if (_Filteredclients == null)
                    _Filteredclients = Clients;

                return _Filteredclients;
            }
            set
            {
                _Filteredclients = value;
                NotifyPropertyChanged(m => m.Filteredclients);
            }
        }
       
       
        private Client selectedClient;
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                //this.Message = selectedClient.NomClient;
                NotifyPropertyChanged(m => m.SelectedClient);
            }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged(m => m.message);
            }
        }

        private string _FilterString;
        public string FilterString
        {
            get { return _FilterString; }
            set
            {
                _FilterString = value;

                Filteredclients = new ObservableCollection<Client>();

                foreach (Client m in Clients)
                {
                    if (m.NomClient.ToLower().Contains(_FilterString.ToLower()))
                    {
                        Filteredclients.Add(m);
                    }
                }

                NotifyPropertyChanged(m => m.FilterString);
            }
        }
        #endregion
        
        #region Methods

        public void LoadClients()
        {
            ServiceAgent.GetClients((clients, error) => ClientLoaded(clients, error));
        }
        private void AddClient()
        {
            string s = "Ajout de";
            string z = NomClient;
            if (NomClient != null && Adresse != null )
                if(CP.HasValue && Telephone.HasValue)
            {
                this.Clients.Insert(0, new Client { NomClient = NomClient, Adresse = Adresse, CP = CP.Value, Telephone = Telephone.Value, Mode = emMode.add });
                ServiceAgent.Flush(this.Clients, (error) => ClientsFlushed(error));
                ServiceAgent.GetClients((clients, error) => ClientLoaded(clients, error));
                ReinitField();
                NotifyError(s + " " + z, null);
            }
        }

        private void UpdateClient()
        {
            foreach (Client _cli in this.Filteredclients)
            {
                _cli.Mode = emMode.update;
            }

            ServiceAgent.Flush(this.Filteredclients, (error) => ClientUpdateSuccess(error));
            ServiceAgent.GetClients((clients, error) => ClientLoaded(clients, error));

        }

        private void DeleteClient()
        {
            string s = "Suppression de";
            string z = SelectedClient.NomClient;
            if (SelectedClient != null)
            {
                this.SelectedClient.Mode = emMode.delete;
                ServiceAgent.Flush(this.Clients, (error) => ClientsFlushed(error));
                ServiceAgent.GetClients((clientlist, error) => ClientLoaded(clientlist, error));
                NotifyError(s +" "+ z,null);
            }
        }

        private void Search()
        {
            ServiceAgent.GetClients((clientlist, error) => ClientLoaded(clientlist, error));
        }

        private void ReinitField()
        {
            NomClient = "";
            Adresse = "";
            CP = null;
            Telephone = null;

        }

        #endregion

        #region Callbacks

        private void ClientLoaded(ObservableCollection<Client> clients, Exception error)
        {
            if (error == null)
            {
                this.Clients = clients;
            }
            else
            {
                NotifyError(error.Message, error);

            }
            // isbusy = false;
        }
        private void ClientsFlushed(Exception error)
        {
            if (error == null)
                NotifyError("Flushed", null);
            else
                NotifyError(error.Message, error);
        }

        private void ClientUpdateSuccess(Exception error)
        {
            if (error == null)
                NotifyError("Modifié", null);
            else
                NotifyError(error.Message, error);
        }

        #endregion
       
        #region Commands

        private DelegateCommand addCommand;
        public DelegateCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                    addCommand = new DelegateCommand(AddClient);
                return addCommand;
            }
            private set
            {
                addCommand = value;
            }
        }

        private DelegateCommand editCommand;
        public DelegateCommand EditCommand
        {
            get
            {
                if (editCommand == null)
                    editCommand = new DelegateCommand(UpdateClient);
                return editCommand;
            }
            private set
            {
                editCommand = value;
            }
        }

        private DelegateCommand deleteCommand;
        public DelegateCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new DelegateCommand(DeleteClient);
                return deleteCommand;
            }
            private set
            {
                deleteCommand = value;
            }
        }

        private DelegateCommand<object> _FilterChange;
        public DelegateCommand<object> FilterChange
        {
            get
            {
                if (_FilterChange == null)
                    _FilterChange = new DelegateCommand<object>(new Action<object>((o) =>
                    {
                        TextBox myTB = o as TextBox;
                        FilterString = myTB.Text;
                    }));

                return _FilterChange;
            }
        }

        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                if (searchCommand == null) searchCommand = new DelegateCommand(Search);
                return searchCommand;
            }
            private set
            {
                searchCommand = value;
            }
        }

        #endregion

        #region Helpers

        // Helper method to notify View of an error
        private void NotifyError(string message, Exception error)
        {
            this.Message = message;
            // Notify view of an error
            Notify(ErrorNotice, new NotificationEventArgs<Exception>(message, error));
        }

        #endregion
}

}
