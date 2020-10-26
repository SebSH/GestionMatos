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
    public class Clients
    {
        public static ObservableCollection<Client> GetClient()
        {
            List<Client> result = new List<Client>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select * from client", conn))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Client client = new Client();
                            client.Id = Convert.ToInt32(rdr["id_client"]);
                            client.NomClient = rdr["Nom_client"].ToString();
                            client.Adresse = rdr["Adresse"].ToString();
                            client.CP = Convert.ToInt32(rdr["CP"]);
                            client.Telephone = Convert.ToInt32(rdr["Telephone"]);

                            result.Add(client);
                        }
                    }
                }
                conn.Close();
            }
            var oc = new ObservableCollection<Client>();
            result.ForEach(x => oc.Add(x));
            return oc;
        }

        internal static void Flush(ObservableCollection<Client> clients)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GestionMatos"].ToString()))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    //ObservableCollection<Product> list = products.Distinct(x => x.Mode != emMode.none);
                    foreach (Client client in clients)
                    {
                        if (client.Mode == emMode.none)
                            continue;
                        else
                            if (client.Mode == emMode.add)
                            {
                                client.Mode = emMode.none;
                                Insert(client, conn, tran);
                                break;
                            }

                            else if (client.Mode == emMode.delete)
                            {
                                client.Mode = emMode.none;
                                Delete(client.Id, conn, tran);
                            }
                            else
                                if (client.Mode == emMode.update)
                                { client.Mode = emMode.none;
                                Update(client, conn, tran);
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
            using (SqlCommand cmd = new SqlCommand("delete from client where id_client = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        private static void Update(Client client, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("update client set Nom_client = @name, Adresse = @adr, CP = @cp, Telephone = @tel where id_client = @id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", client.Id);
                cmd.Parameters.AddWithValue("@name", client.NomClient);
                cmd.Parameters.AddWithValue("@adr", client.Adresse);
                cmd.Parameters.AddWithValue("@cp", client.CP);
                cmd.Parameters.AddWithValue("@tel", client.Telephone);
                cmd.ExecuteNonQuery();
            }
        }

        private static void Insert(Client client, SqlConnection conn, SqlTransaction tran)
        {
            using (SqlCommand cmd = new SqlCommand("insert into client(Nom_client, Adresse, CP, Telephone) values (@name, @adr, @cp, @tel)", conn, tran))
            {
                cmd.Parameters.AddWithValue("@name", client.NomClient);
                cmd.Parameters.AddWithValue("@adr", client.Adresse);
                cmd.Parameters.AddWithValue("@cp", client.CP);
                cmd.Parameters.AddWithValue("@tel", client.Telephone);
                cmd.ExecuteNonQuery();
            }
        }
    }


    public class Client : ModelBase<Client>
    {
        public int Id { get; set; }
        public string NomClient { get; set; }
        public string Adresse { get; set; }
        public int CP { get; set; }
        public int Telephone { get; set; }
        public emMode Mode { get; set; }
    }
    public enum emMode { update, add, delete, none };

}