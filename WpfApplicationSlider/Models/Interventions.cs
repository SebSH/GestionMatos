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
    class Interventions
    {
        public static ObservableCollection<Interv> GetInterv()
        {
            List<Interv> result = new List<Interv>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from intervention join materiel on intervention.id_materiel = materiel.id_materiel join client on materiel.id_client= client.id_client  join  site on materiel.id_site= site.id_site join batiment on site.id_bat = batiment.id_bat join etage on batiment.id_et = etage.id_et join salle on etage.id_sal = salle.id_sal  ", conn))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Interv interv = new Interv();
                            interv.Id = Convert.ToInt32(rdr["id_interv"]);
                            interv.Numero = Convert.ToInt32(rdr["Numero_interv"]);
                            interv.Dateplan = Convert.ToDateTime(rdr["Dateplan"]);
                            interv.Datereal = Convert.ToDateTime(rdr["Datereal"]);
                            interv.Commentaire = rdr["Commentaire"].ToString();
                            interv.IdMateriel = Convert.ToInt32(rdr["id_materiel"]);
                            interv.NomMateriel = rdr["Nom"].ToString();
                            interv.Dateinterv = Convert.ToDateTime(rdr["date_interv"]);
                            interv.IdClient = Convert.ToInt32(rdr["id_client"]);
                            interv.NomClient = rdr["Nom_client"].ToString();
                            interv.IdSite = Convert.ToInt32(rdr["id_site"]);
                            interv.NomSite = rdr["Nom_site"].ToString();
                            interv.IdBatiment = Convert.ToInt32(rdr["id_bat"]);
                            interv.Batiment = Convert.ToInt32(rdr["NumeroB"]);
                            interv.IdEtage = Convert.ToInt32(rdr["id_et"]);
                            interv.Etage = Convert.ToInt32(rdr["NumeroE"]);
                            interv.IdSalle = Convert.ToInt32(rdr["id_sal"]);
                            interv.Salle = Convert.ToInt32(rdr["NumeroS"]);

                            result.Add(interv);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Interv>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }

        internal static void Flush(ObservableCollection<Interv> intervs)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    foreach (Interv interv in intervs)
                    {
                        if (interv.Mode == emMode4.none)
                            continue;
                        else
                            if (interv.Mode == emMode4.add)
                            {
                                interv.Mode = emMode4.none;
                                Insert(interv, conn, tran);
                                break;
                            }

                            else if (interv.Mode == emMode4.delete)
                            {
                                interv.Mode = emMode4.none;
                                Delete(interv.Id, conn, tran);
                            }
                            else
                                if (interv.Mode == emMode4.update)
                                { interv.Mode = emMode4.none; continue; }
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

        private static void Delete(int id, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("delete from intervention where id_interv = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        private static void Update(Interv interv, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("update materiel set Numero = @num, MTBF = @mtbf where id_materiel = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", interv.Id);
                cmd.Parameters.AddWithValue("@name", interv.NomMateriel);
                cmd.Parameters.AddWithValue("@num", interv.Numero);
                cmd.ExecuteNonQuery();
            }
        }

        private static void Insert(Interv interv, SqlConnection conn, SqlTransaction tran)
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd");

            using (SqlCommand cmd = new SqlCommand("insert into intervention( Numero_interv, Dateplan, Commentaire, Nom_materiel) values (@num, (@datep, @formatedDate), @com, @nommat)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", interv.Id);
                cmd.Parameters.AddWithValue("@num", interv.Numero);
                cmd.Parameters.AddWithValue("@datep", interv.Dateplan);
                cmd.Parameters.AddWithValue("@com", interv.Commentaire);
                cmd.Parameters.AddWithValue("@nommat", interv.NomMateriel);
                cmd.Parameters.AddWithValue("@formatedDate", sqlFormattedDate);
                cmd.ExecuteNonQuery();
            }


        }
    }

    public class Interv : ModelBase<Interv>
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public DateTime Dateplan { get; set; }
        public DateTime Datereal { get; set; }
        public string Commentaire { get; set; }
        public int Valide { get; set; }
        public string NomMateriel { get; set; }
        public DateTime Dateinterv { get; set; }
        public int IdMateriel { get; set; }
        public string NomClient { get; set; }
        public int IdClient { get; set; }
        public string NomSite { get; set; }
        public int IdSite { get; set; }
        public int Batiment { get; set; }
        public int IdBatiment { get; set; }
        public int Etage { get; set; }
        public int IdEtage { get; set; }
        public int Salle { get; set; }
        public int IdSalle { get; set; }
        public emMode4 Mode { get; set; }
    }
    public enum emMode4 { update, add, delete, none };
}
