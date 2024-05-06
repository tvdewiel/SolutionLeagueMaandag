using League.BL.Domein;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Repositories
{
    public class SpelerRepositoryADO : ISpelerRepository
    {
        private string connectionString;

        public SpelerRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatSpeler(Speler s)
        {
            string query = "SELECT count(*) FROM speler WHERE naam=@naam";
            using(SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@naam", s.Naam);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    return false;
                }
                catch(Exception ex) { throw new RepositoryException("BestaatSpeler"); }
            }
        }

        public Speler SchrijfSpelerInDB(Speler s)
        {
            string query = "INSERT INTO Speler(naam,lengte,gewicht) output INSERTED.ID VALUES(@naam,@lengte,@gewicht)";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@naam", s.Naam);
                    if (s.Lengte==null) cmd.Parameters.AddWithValue("@lengte",DBNull.Value);
                    else cmd.Parameters.AddWithValue("@lengte", s.Lengte);
                    if (s.Gewicht == null) cmd.Parameters.AddWithValue("@gewicht", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@gewicht", s.Gewicht);

                    int newID=(int)cmd.ExecuteScalar();
                    s.ZetId(newID); 
                    return s;
                }
                catch (Exception ex) { throw new RepositoryException("SchrijfSpeler"); }
            }
        }
    }
}
