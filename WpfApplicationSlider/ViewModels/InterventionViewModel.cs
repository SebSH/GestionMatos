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
    class InterventionViewModel : ViewModelBase<InterventionViewModel>
    {
        public InterventionViewModel() { }

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



        private string commentaire;
        public string Commentaire
        {
            get { return commentaire; }
            set
            {
                commentaire = value;
                NotifyPropertyChanged(m => m.Commentaire);
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

        private int idbat;
        public int IdBat
        {
            get { return idbat; }
            set
            {
                idbat = value;
                NotifyPropertyChanged(m => m.IdBat);
            }
        }

        private int idet;
        public int IdEt
        {
            get { return idet; }
            set
            {
                idet = value;
                NotifyPropertyChanged(m => m.IdEt);
            }
        }

        private int idsal;
        public int IdSal
        {
            get { return idsal; }
            set
            {
                idsal = value;
                NotifyPropertyChanged(m => m.IdSal);
            }
        }

        private int? batiment;
        public int? Batiment
        {
            get { return batiment; }
            set
            {
                batiment = value;
                NotifyPropertyChanged(m => m.Batiment);
            }
        }

        private int? etage;
        public int? Etage
        {
            get { return etage; }
            set
            {
                etage = value;
                NotifyPropertyChanged(m => m.Etage);
            }
        }

        private int? salle;
        public int? Salle
        {
            get { return salle; }
            set
            {
                salle = value;
                NotifyPropertyChanged(m => m.Salle);
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
        public int IdClient
        {
            get { return idclient; }
            set
            {
                idclient = value;
                NotifyPropertyChanged(m => m.IdClient);
            }
        }

        private int idmateriel;
        public int IdMateriel
        {
            get { return idmateriel; }
            set
            {
                idmateriel = value;
                NotifyPropertyChanged(m => m.IdMateriel);
            }
        }

        private int valide;
        public int Valide
        {
            get { return valide; }
            set
            {
                valide = value;
                NotifyPropertyChanged(m => m.Valide);
            }
        }

        private DateTime? dateplan;
        public DateTime? Dateplan
        {
            get { return dateplan; }
            set
            {
                dateplan = value;
                NotifyPropertyChanged(m => m.Dateplan);
            }
        }

        private DateTime datereal;
        public DateTime Datereal
        {
            get { return datereal; }
            set
            {
                datereal = value;
                NotifyPropertyChanged(m => m.Datereal);
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

        public ObservableCollection<Interv> Intervs
        {
            get { return MainViewModel.GetInstance().Intervs; }
            set
            {
                if ((value != null) && (value != MainViewModel.GetInstance().Intervs))
                {
                    MainViewModel.GetInstance().Intervs = value;
                    NotifyPropertyChanged(m => m.Intervs);
                }
            }
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

                else return new ObservableCollection<Site>();
            }

            set
            {
                filteredsites = value;


            }
        }

        private ObservableCollection<Materiel> filteredmateriels;
        public ObservableCollection<Materiel> FilteredMateriels
        {
            get
            {
                if (selectedClient != null)
                {
                    return filteredmateriels;
                }

                else return new ObservableCollection<Materiel>();
            }

            set
            {
                filteredmateriels = value;


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

        private Materiel selectedIntervention;
        public Materiel SelectedIntervention
        {
            get { return selectedIntervention; }
            set
            {
                selectedIntervention = value;
                NotifyPropertyChanged(m => m.SelectedIntervention);
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
                LoadFilteredMateriel();
                NotifyPropertyChanged(m => m.FilteredMateriels);
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

        public void LoadInterventions()
        {
            ServiceAgentI.GetInterventions((intervs, error) => InterventionLoaded(intervs, error));
        }

        public void LoadFilteredSites()
        {
            if (SelectedClient != null)
            {
                IdClient = this.selectedClient.Id;
                ServiceAgentS.GetFilteredSites((filteredsites, error) => FilteredSitesLoaded(filteredsites, error), IdClient);

            }
        }

        public void LoadFilteredMateriel()
        {
            if (SelectedClient != null && SelectedSite != null)
            {
                IdClient = this.selectedClient.Id;
                IdSite = this.selectedSite.Id;
                ServiceAgentM.GetFilteredMateriel((filteredmateriels, error) => FilteredMaterielLoaded(filteredmateriels, error), IdClient, IdSite);

            }
        }
        private void AddIntervention()
        {
           
                
                    IdClient = this.SelectedClient.Id;
                    NomClient = this.SelectedClient.NomClient;
                    IdMateriel = this.SelectedMateriel.Idm;
                    NomMateriel = this.SelectedMateriel.NomMateriel;
                    IdSite = this.SelectedSite.Id;
                    NomSite = this.selectedSite.NomSite;
                    this.Intervs.Insert(0, new Interv { Dateplan = Dateplan.Value, NomMateriel = NomMateriel, Numero = Numero.Value, NomSite = NomSite, NomClient = NomClient, IdClient = IdClient, IdSite = IdSite, IdBatiment = IdBat, IdEtage = IdEt, IdSalle = IdSal, Mode = emMode4.add });
                    ServiceAgentI.Flush(this.Intervs, (error) => InterventionsFlushed(error));
                    ServiceAgentI.GetInterventions((intervs, error) => InterventionLoaded(intervs, error));
                    ReinitField();
                   
                
        }

        private void EditIntervention()
        {
            this.SelectedIntervention.NomMateriel = "Edited Name";

            //this.SelectedIntervention.Mode = emMode4.none;
            ServiceAgentI.Flush(this.Intervs, (error) => InterventionsFlushed(error));
        }

        private void DeleteIntervention()
        {
            if (SelectedIntervention != null)
            {
                //this.SelectedIntervention.Mode = emMode4.delete;
                ServiceAgentI.Flush(this.Intervs, (error) => InterventionsFlushed(error));
                ServiceAgentI.GetInterventions((intervlist, error) => InterventionLoaded(intervlist, error));
            }
        }

        private void ReinitField()
        {
            NomMateriel = "";
            Commentaire = "";
            Numero = null;
            Dateplan = default(DateTime);
            Datereal = default(DateTime);

        }

        #endregion

        #region Callbacks

        private void InterventionLoaded(ObservableCollection<Interv> intervs, Exception error)
        {
            if (error == null)
            {
                this.Intervs = intervs;
                NotifyPropertyChanged(m => m.Intervs);
                NotifyError("", null);
            }
            else
            {
                NotifyError(error.Message, error);

            }
            
        }
        private void InterventionsFlushed(Exception error)
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

        private void FilteredMaterielLoaded(ObservableCollection<Materiel> filteredmateriels, Exception error)
        {
            if (error == null)
            {
                this.FilteredMateriels = filteredmateriels;
                NotifyPropertyChanged(m => m.FilteredMateriels);

            }
            else
            {
                NotifyError(error.Message, error);

            }
        }

        #endregion

        #region Commands

        private DelegateCommand addCommand;
        public DelegateCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                    addCommand = new DelegateCommand(AddIntervention);
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
                    editCommand = new DelegateCommand(EditIntervention);
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
                    deleteCommand = new DelegateCommand(DeleteIntervention);
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
