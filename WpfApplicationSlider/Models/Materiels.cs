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

    public class Materiels
    {
        public static ObservableCollection<Materiel> GetMateriel()
        {
            List<Materiel> result = new List<Materiel>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from materiel join site on materiel.id_site=site.id_site join client on materiel.id_client=client.id_client", conn))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Materiel materiel = new Materiel();
                            materiel.Idm = Convert.ToInt32(rdr["id_materiel"]);
                            materiel.NomMateriel = rdr["Nom"].ToString();
                            materiel.Description = rdr["Description"].ToString();
                            materiel.Numero = Convert.ToInt32(rdr["Numero"]);
                            materiel.MTBF = Convert.ToInt32(rdr["MTBF"]);
                            materiel.NomSite = rdr["Nom_site"].ToString();
                            materiel.NomClient = rdr["Nom_client"].ToString();
                            materiel.Dateinterv = Convert.ToDateTime(rdr["date_interv"]);
                            result.Add(materiel);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Materiel>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }

        public static ObservableCollection<Materiel> GetFilteredMateriel(int SelectedClient, int SelectedSite)
        {
            List<Materiel> result = new List<Materiel>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from materiel join site on materiel.id_site=site.id_site join client on materiel.id_client=client.id_client where client.id_client=@selectedclient AND site.id_site = @selectedsite", conn))
                {

                    cmd.Parameters.AddWithValue("@selectedclient", SelectedClient);
                    cmd.Parameters.AddWithValue("@selectedsite", SelectedSite);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Materiel filteredmateriels = new Materiel();
                            filteredmateriels.Idm = Convert.ToInt32(rdr["id_materiel"]);
                            filteredmateriels.NomMateriel = rdr["Nom"].ToString();
                            result.Add(filteredmateriels);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Materiel>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }


        internal static void Flush(ObservableCollection<Materiel> materiels)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                   
                    foreach (Materiel materiel in materiels)
                    {
                        if (materiel.Mode == emMode2.none)
                            continue;
                        else
                            if (materiel.Mode == emMode2.add)
                            {
                                materiel.Mode = emMode2.none;
                                Insert(materiel, conn, tran);
                                break;
                            }

                            else if (materiel.Mode == emMode2.delete)
                            {
                                materiel.Mode = emMode2.none;
                                Delete(materiel.Idm, conn, tran);
                            }
                            else
                                if (materiel.Mode == emMode2.update)
                                {
                                    materiel.Mode = emMode2.none;
                                    Update(materiel, conn, tran);
                                    continue;
                                }
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
            using (SqlCommand cmd = new SqlCommand("delete from materiel where id_materiel = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        private static void Update(Materiel materiel, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("update materiel set Nom = @name, Description = @des, Numero = @num, MTBF = @mtbf,  where id_materiel = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", materiel.Idm);
                cmd.Parameters.AddWithValue("@name", materiel.NomMateriel);
                cmd.Parameters.AddWithValue("@des", materiel.Description);
                cmd.Parameters.AddWithValue("@num", materiel.Numero);
                cmd.Parameters.AddWithValue("@mtbf", materiel.MTBF);
                cmd.Parameters.AddWithValue("@dint", materiel.Dateinterv);
                cmd.Parameters.AddWithValue("@idint", materiel.Idinterv);
                cmd.ExecuteNonQuery();
            }
        }

        private static void Insert(Materiel materiel, SqlConnection conn, SqlTransaction tran)
        {
            int mtbf = materiel.MTBF;
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd");

            using (SqlCommand cmd = new SqlCommand("insert into materiel(Nom, Description, Numero, MTBF, id_site, id_client, date_interv) values (@name, @des, @num, @mtbf, @idsite, @idclient, (DATEADD(year,@mtbf,@formatedDate)))", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", materiel.Idm);
                cmd.Parameters.AddWithValue("@name", materiel.NomMateriel);
                cmd.Parameters.AddWithValue("@des", materiel.Description);
                cmd.Parameters.AddWithValue("@num", materiel.Numero);
                cmd.Parameters.AddWithValue("@mtbf", materiel.MTBF);
                cmd.Parameters.AddWithValue("@idsite", materiel.Idsite);
                cmd.Parameters.AddWithValue("@idclient", materiel.Idclient);
                cmd.Parameters.AddWithValue("@formatedDate", sqlFormattedDate);
                cmd.ExecuteNonQuery();
            }

            
        }
    }


    public class Materiel : ModelBase<Materiel>
    {
        public int Idm { get; set; }
        public string NomMateriel { get; set; }
        public string Description { get; set; }
        public int Numero { get; set; }
        public int MTBF { get; set; }
        public DateTime Dateinterv { get; set; }
        public string NomSite { get; set; }
        public string NomClient { get; set; }
        public int Idinterv { get; set; }
        public int Idtype { get; set; }
        public int Idclient { get; set; }
        public int Idsite { get; set; }
        public emMode2 Mode { get; set; }
    }
    public enum emMode2 { update, add, delete, search, none };

}
