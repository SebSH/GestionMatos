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
    class MaterielViewModel:ViewModelBase<MaterielViewModel>
    {
        public MaterielViewModel() { }
        
        #region Notifications

        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        #endregion

        #region Properties

      
        private string nomMateriel;
        public string NomMateriel
        {
            get { return nomMateriel; }
            set
            {
                nomMateriel = value;
                NotifyPropertyChanged(m => m.NomMateriel);
            }
        }



        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged(m => m.Description);
            }
        }

        private int? numero;
        public int? Numero
        {
            get { return numero; }
            set
            {
                numero = value;
                NotifyPropertyChanged(m => m.Numero);
            }
        }

        private int? mtbf;
        public int? MTBF
        {
            get { return mtbf; }
            set
            {
                mtbf = value;
                NotifyPropertyChanged(m => m.MTBF);
            }
        }
        private string nomSite;
        public string NomSite
        {
            get { return nomSite; }
            set
            {
                nomSite = value;
                NotifyPropertyChanged(m => m.NomSite);
            }
        }

        private int idsite;
        public int IdSite
        {
            get { return idsite; }
            set
            {
                idsite = value;
                NotifyPropertyChanged(m => m.IdSite);
            }
        }

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

        private int idclient;
        public int Idclient
        {
            get { return idclient; }
            set
            {
                idclient = value;
                NotifyPropertyChanged(m => m.Idclient);
            }
        }


        public ObservableCollection<Materiel> Materiels
        {
            get { return MainViewModel.GetInstance().Materiels; }
            set
            {
                if ((value != null) && (value != MainViewModel.GetInstance().Materiels))
                {
                    MainViewModel.GetInstance().Materiels = value;
                    NotifyPropertyChanged(m => m.Materiels);
                }
            }
        }
        
        public ObservableCollection<Client> Clients
        {
            get { return MainViewModel.GetInstance().Clients; }

        }

        public ObservableCollection<Site> Sites
        {
            get { return MainViewModel.GetInstance().Sites; }

        }

        private ObservableCollection<Site> filteredsites;
        public ObservableCollection<Site> FilteredSites
        {
            get
            {
                if (selectedClient != null)
                {
                    return filteredsites;
                }

                else  return new ObservableCollection<Site>(); 
            }

            set
            {
                filteredsites = value;


            }
        }


        private ObservableCollection<Materiel> _Filteredmateriels;
        public ObservableCollection<Materiel> Filteredmateriels
        {
            get
            {

                if (_Filteredmateriels == null)
                    _Filteredmateriels = Materiels;

                return _Filteredmateriels;
            }
            set
            {
                _Filteredmateriels = value;
                NotifyPropertyChanged(m => m.Filteredmateriels);
            }
        }
        private Materiel selectedMateriel;
        public Materiel SelectedMateriel
        {
            get { return selectedMateriel; }
            set
            {
                selectedMateriel = value;
                NotifyPropertyChanged(m => m.SelectedMateriel);
            }
        }

        private Site selectedSite;
        public Site SelectedSite
        {
            get { return selectedSite; }
            set
            {
                selectedSite = value;
                NotifyPropertyChanged(m => m.SelectedSite);
            }
        }

       

        private Client selectedClient;
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                LoadFilteredSites();
                NotifyPropertyChanged(m => m.FilteredSites);
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

                Filteredmateriels = new ObservableCollection<Materiel>();

                //1
                foreach (Materiel m in Materiels)
                {
                    if (m.NomMateriel.ToLower().Contains(_FilterString.ToLower()))
                    {
                        Filteredmateriels.Add(m);
                    }
                }

                NotifyPropertyChanged(m => m.FilterString);
            }
        }
        #endregion
        
        #region Methods

        public void LoadMateriels()
        {
            ServiceAgentM.GetMateriels((materiels, error) => MaterielLoaded(materiels, error));
        }

        public void LoadFilteredSites()
        {
            if (SelectedClient != null)
            {
                Idclient = this.selectedClient.Id;
                ServiceAgentS.GetFilteredSites((filteredsites, error) => FilteredSitesLoaded(filteredsites, error), Idclient);

            }
        }
        private void AddMateriel()
        {
            string s = "Ajout de";
            string z = NomMateriel;
            if (NomMateriel != null && Description != null )
                if(Numero.HasValue && MTBF.HasValue)
            {
                Idclient = this.SelectedClient.Id;
                NomClient = this.SelectedClient.NomClient;
                IdSite = this.SelectedSite.Id;
                NomSite = this.selectedSite.NomSite;
                this.Materiels.Insert(0, new Materiel { NomMateriel = NomMateriel, Description = Description, Numero = Numero.Value, MTBF = MTBF.Value, NomSite=NomSite, NomClient=NomClient, Idclient=Idclient, Idsite=IdSite,  Mode = emMode2.add });
                ServiceAgentM.Flush(this.Materiels, (error) => MaterielsFlushed(error));
                ServiceAgentM.GetMateriels((materiels, error) => MaterielLoaded(materiels, error));
                LoadMateriels();
                ReinitField();
                NotifyError(s + " " + z, null);
            }
        }

        private void UpdateMateriel()
        {
            foreach (Materiel _cli in this.Materiels)
            {
                _cli.Mode = emMode2.update;
            }

            ServiceAgentM.Flush(this.Materiels, (error) => MaterielUpdateSuccess(error));
            ServiceAgentM.GetMateriels((clients, error) => MaterielLoaded(clients, error));

        }


        private void DeleteMateriel()
        {
            string s = "Suppression de";
            string z = SelectedMateriel.NomMateriel;
            if (SelectedMateriel != null)
            {
                this.SelectedMateriel.Mode = emMode2.delete;
                ServiceAgentM.Flush(this.Materiels, (error) => MaterielsFlushed(error));
                ServiceAgentM.GetMateriels((materiellist, error) => MaterielLoaded(materiellist, error));
                NotifyError(s +" "+ z,null);
            }
        }

        private void ReinitField()
        {
            NomMateriel = "";
            Description = "";
            Numero = null;
            MTBF = null;

        }
    
        #endregion

        #region Callbacks

        private void MaterielLoaded(ObservableCollection<Materiel> materiels, Exception error)
        {
            if (error == null)
            {
                this.Materiels = materiels;
                NotifyPropertyChanged(m =>m.Materiels);
                NotifyError("", null);
            }
            else
            {
                NotifyError(error.Message, error);

            }
            // isbusy = false;
        }
        private void MaterielsFlushed(Exception error)
        {
            if (error == null)
                NotifyError("Flushed", null);
            else
                NotifyError(error.Message, error);
        }

        private void FilteredSitesLoaded(ObservableCollection<Site> filteredsites, Exception error)
        {
            if (error == null)
            {
                this.FilteredSites = filteredsites;
                NotifyPropertyChanged(m => m.FilteredSites);
               
            }
            else
            {
                NotifyError(error.Message, error);

            }
        }

        private void MaterielUpdateSuccess(Exception error)
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
                    addCommand = new DelegateCommand(AddMateriel);
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
                    editCommand = new DelegateCommand(UpdateMateriel);
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
                    deleteCommand = new DelegateCommand(DeleteMateriel);
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
