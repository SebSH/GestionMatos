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
    class SiteViewModel:ViewModelBase<SiteViewModel>
    {
      
            public SiteViewModel() { }

            #region Notifications

            public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

            #endregion

            #region Properties


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

            private int idsalle;
            public int Idsalle
            {
                get { return idsalle; }
                set
                {
                    idsalle = value;
                    NotifyPropertyChanged(m => m.Idsalle);
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

            public ObservableCollection<Site> Sites
            {
                get { return MainViewModel.GetInstance().Sites; }
                set
                {
                    if ((value != null) && (value != MainViewModel.GetInstance().Sites))
                    {
                        MainViewModel.GetInstance().Sites = value;
                        NotifyPropertyChanged(m => m.Sites);
                    }
                }
            }
            public ObservableCollection<Client> Clients
            {
                get { return MainViewModel.GetInstance().Clients; }
                
            }

            private ObservableCollection<Site> _Filteredsites;
            public ObservableCollection<Site> Filteredsites
            {
                get
                {

                    if (_Filteredsites == null)
                        _Filteredsites = Sites;

                    return _Filteredsites;
                }
                set
                {
                    _Filteredsites = value;
                    NotifyPropertyChanged(m => m.Filteredsites);
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

                    Filteredsites = new ObservableCollection<Site>();

                    //1
                    foreach (Site m in Sites)
                    {
                        if (m.NomSite.ToLower().Contains(_FilterString.ToLower()))
                        {
                            Filteredsites.Add(m);
                        }
                    }

                    NotifyPropertyChanged(m => m.FilterString);
                }
            }
            #endregion

            #region Methods

            public void LoadSites()
            {
                ServiceAgentS.GetSites((sites, error) => SiteLoaded(sites, error));
            }
            private void AddSite()
            {
                string s = "Ajout de";
                string z = NomSite;

                        Idclient = this.SelectedClient.Id;
                        NomClient = this.selectedClient.NomClient;
                        this.Sites.Insert(0, new Site { NomSite = NomSite, NomClient = NomClient, Adresse = Adresse, Batiment = Batiment.Value, Etage = Etage.Value, Salle = Salle.Value, idclient=Idclient, Mode = emMode3.add });
                        ServiceAgentS.Flush(this.Sites, (error) => SitesFlushed(error));
                        ReinitField();
                        NotifyError(s + " " + z, null);
                    
            }

            private void EditSite()
            {
                this.SelectedSite.NomSite = "Edited Name";

                this.SelectedSite.Mode = emMode3.none;
                ServiceAgentS.Flush(this.Sites, (error) => SitesFlushed(error));
            }

            private void DeleteSite()
            {
                string s = "Suppression de";
                string z = SelectedSite.NomSite;
                if (SelectedSite != null)
                {
                    this.SelectedSite.Mode = emMode3.delete;
                    ServiceAgentS.Flush(this.Sites, (error) => SitesFlushed(error));
                    ServiceAgentS.GetSites((sitelist, error) => SiteLoaded(sitelist, error));
                    NotifyError(s + " " + z, null);
                }
            }

            private void Search()
            {
                ServiceAgentS.GetSites((sitelist, error) => SiteLoaded(sitelist, error));
            }

            private void ReinitField()
            {
                NomSite = "";
                NomClient = "";
                Adresse = "";
                Batiment = null;
                Etage = null;
                Salle = null;

            }
        
            #endregion

            #region Callbacks

            private void SiteLoaded(ObservableCollection<Site> sites, Exception error)
            {
                if (error == null)
                {
                    this.Sites = sites;
                    NotifyPropertyChanged(m => m.Sites);
                    NotifyError("", null);
                }
                else
                {
                    NotifyError(error.Message, error);

                }
                // isbusy = false;
            }
            private void SitesFlushed(Exception error)
            {
                if (error == null)
                    NotifyError("Flushed", null);
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
                        addCommand = new DelegateCommand(AddSite);
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
                        editCommand = new DelegateCommand(EditSite);
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
                        deleteCommand = new DelegateCommand(DeleteSite);
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

