using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using SimpleMvvmToolkit;

namespace WpfApplicationSlider.Models
{
    public class Sites
    {
        public static ObservableCollection<Site> GetSite()
        {
            List<Site> result = new List<Site>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from site join batiment on site.id_bat=batiment.id_bat join etage on batiment.id_et=etage.id_et join salle on etage.id_sal=salle.id_sal join situe on situe.id_site=site.id_site join client on client.id_client=situe.id_client", conn))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Site site = new Site();
                            site.Id = Convert.ToInt32(rdr["id_site"]);
                            site.NomSite = rdr["Nom_site"].ToString();
                            site.NomClient = rdr["Nom_client"].ToString();
                            site.Adresse = rdr["Adresse_site"].ToString();
                            site.Batiment = Convert.ToInt32(rdr["NumeroB"]);
                            site.Etage = Convert.ToInt32(rdr["NumeroE"]);
                            site.Salle = Convert.ToInt32(rdr["NumeroS"]);
                            result.Add(site);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Site>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }


        public static ObservableCollection<Site> GetFilteredSite(int SelectedClient)
        {
            List<Site> result = new List<Site>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select * from site join situe on situe.id_site=site.id_site join client on client.id_client=situe.id_client where client.id_client=@selectedclient", conn))
                {

                    cmd.Parameters.AddWithValue("@selectedclient", SelectedClient);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Site filteredsites = new Site();
                            filteredsites.Id = Convert.ToInt32(rdr["id_site"]);
                            filteredsites.NomSite = rdr["Nom_site"].ToString();
                            result.Add(filteredsites);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Site>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }


        internal static void Flush(ObservableCollection<Site> sites)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                  
                    foreach (Site site in sites)
                    {
                        if (site.Mode == emMode3.none)
                            continue;
                        else
                            if (site.Mode == emMode3.add)
                            {
                                site.Mode = emMode3.none;
                                Insert(site, conn, tran);
                                break;
                            }

                            else if (site.Mode == emMode3.delete)
                            {
                                site.Mode = emMode3.none;
                                Delete(site, conn, tran);
                            }
                            else
                                if (site.Mode == emMode3.update)
                                { site.Mode = emMode3.none; continue; }
                    }
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                conn.Close();
            }
        }
        #region Insert
        private static void Insert(Site site, SqlConnection conn, SqlTransaction tran)
        {

            using (SqlCommand cmd = new SqlCommand("insert into salle(NumeroS) values(@num)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@num", site.Salle);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("select id_sal from salle", conn, tran))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {


                    site.idsalle = Convert.ToInt32(rdr["id_sal"]);

                }
            }
            using (SqlCommand cmd = new SqlCommand("insert into etage(NumeroE, id_sal) values(@num, @idsal)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@num", site.Etage);
                cmd.Parameters.AddWithValue("@idsal", site.idsalle);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("select id_et from etage", conn, tran))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {


                    site.idetage = Convert.ToInt32(rdr["id_et"]);

                }
            }
            using (SqlCommand cmd = new SqlCommand("insert into batiment(NumeroB, id_et) values(@num, @idet)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@num", site.Batiment);
                cmd.Parameters.AddWithValue("@idet", site.idetage);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("select id_bat from batiment", conn, tran))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {


                    site.idbatiment = Convert.ToInt32(rdr["id_bat"]);

                }
            }
            using (SqlCommand cmd = new SqlCommand("insert into site(Nom_site, Adresse_site, id_bat) values(@nom, @adr, @idbat)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@nom", site.NomSite);
                cmd.Parameters.AddWithValue("@adr", site.Adresse);
                cmd.Parameters.AddWithValue("@idbat", site.idbatiment);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("select id_site from site", conn, tran))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {


                    site.Id = Convert.ToInt32(rdr["id_site"]);

                }
            }
            using (SqlCommand cmd = new SqlCommand("select id_client from client where id_client=@idclient", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idclient", site.idclient);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {


                        site.idclient = Convert.ToInt32(rdr["id_client"]);

                    }
                }
            }

            using (SqlCommand cmd = new SqlCommand("insert into situe(id_site, id_client) values(@idsite, @idclient)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idsite", site.Id);
                cmd.Parameters.AddWithValue("@idclient", site.idclient);

                cmd.ExecuteNonQuery();
            }

        }

        #endregion

        #region Delete

        private static void Delete(Site site, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("delete from Situe where id_site = @idsite", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idsite", site.Id);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("delete from site where id_site = @idsite", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idsite", site.Id);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("delete from batiment where id_bat = @idbat", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idbat", site.idbatiment);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("delete from etage where id_et = @idet", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idet", site.idetage);
                cmd.ExecuteNonQuery();
            }
            using (SqlCommand cmd = new SqlCommand("delete from salle where id_sal = @idsal", conn, tran))
            {
                cmd.Parameters.AddWithValue("@idsal", site.idsalle);
                cmd.ExecuteNonQuery();
            }
        #endregion

        }
    }
}

        public class Site : ModelBase<Site>
        {
            public int Id { get; set; }
            public string NomSite { get; set; }
            public string NomClient { get; set; }
            public int idclient { get; set; }
            public string Adresse { get; set; }
            public int Batiment { get; set; }
            public int idbatiment { get; set; }
            public int Etage { get; set; }
            public int idetage { get; set; }
            public int Salle { get; set; }
            public int idsalle { get; set; }
            public emMode3 Mode { get; set; }
        }

        public enum emMode3 { update, add, delete, none };

    

