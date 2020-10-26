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
    class Utilisateurs
    {
        public static ObservableCollection<Util> GetUtil()
        {
            List<Util> result = new List<Util>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select * from membre", conn))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Util util = new Util();
                            util.Id = Convert.ToInt32(rdr["idm"]);
                            util.NomUtil = rdr["Nom"].ToString();
                            util.Mdp = rdr["Mdp"].ToString();
                            util.Statut = Convert.ToInt32(rdr["Statut"]);
                            result.Add(util);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Util>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }

       
    public class Util : ModelBase<Util>
    {
        public int Id { get; set; }
        public string NomUtil { get; set; }
        public string Mdp { get; set; }
        public int Statut { get; set; }
    }
}
}
