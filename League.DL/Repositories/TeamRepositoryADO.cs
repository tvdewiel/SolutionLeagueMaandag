using League.BL.Domein;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Repositories
{
    public class TeamRepositoryADO : ITeamRepository
    {
        private string connectionString;

        public TeamRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatTeam(Team team)
        {
            string query = "SELECT count(*) FROM team WHERE stamnummer=@stamnummer";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@stamnummer", team.Stamnummer);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex) { throw new RepositoryException("BestaatTeam"); }
            }
        }

        public void SchrijfTeamInDB(Team team)
        {
            string query = "INSERT INTO team(stamnummer,naam,bijnaam) VALUES(@stamnummer,@naam,@bijnaam)";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@stamnummer", team.Stamnummer);
                    cmd.Parameters.AddWithValue("@naam", team.Naam);
                    if (team.Bijnaam == null) cmd.Parameters.AddWithValue("@bijnaam", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@bijnaam",team.Bijnaam);                   
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new RepositoryException("SchrijfTeam", ex); }
            }
        }

        public Team SelecteerTeam(int stamnummer)
        {
            string sql = "SELECT t1.naam as ploegnaam,t1.Bijnaam,t2.* FROM team t1 left join speler t2 on t1.Stamnummer=t2.Team_id where t1.Stamnummer=@stamnummer";
            using(SqlConnection con = new SqlConnection(connectionString))
            using(SqlCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText= sql;
                cmd.Parameters.AddWithValue("@stamnummer", stamnummer);
                IDataReader reader= cmd.ExecuteReader();
                Team team=null;
                while(reader.Read())
                {
                    //team maken
                    if (team == null)
                    {
                        team = new Team(stamnummer, (string)reader["ploegnaam"]);
                        if (!reader.IsDBNull(reader.GetOrdinal("bijnaam")))
                        {
                            string bijnaam = (string)reader["bijnaam"];
                            team.ZetBijnaam(bijnaam);
                        }
                    }
                    //speler maken + toevoegen
                    if (!reader.IsDBNull(reader.GetOrdinal("id")))
                    {
                        int? lengte = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int)reader["lengte"];
                        int? gewicht = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int)reader["gewicht"];
                        int? rugnummer = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("rugnummer"))) rugnummer = (int)reader["rugnummer"];
                        Speler speler = new Speler((int)reader["id"], (string)reader["naam"], lengte, gewicht);
                        if (rugnummer != null) speler.ZetRugnummer((int)rugnummer);
                        speler.ZetTeam(team);
                    }
                }
                reader.Close();
                return team;
            }
        }
    }
}
