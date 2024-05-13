using League.BL.Domein;
using League.BL.DTO;
using League.BL.Interfaces;
using League.DL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
                    int n=(int)cmd.ExecuteScalar();
                    if (n>0) return true; else return false;
                }
                catch(Exception ex) { throw new RepositoryException("BestaatSpeler"); }
            }
        }
        public Speler SchrijfSpelerInDB(Speler s)
        {
            string query = "INSERT INTO Speler(naam,lengte,gewicht) output INSERTED.ID  VALUES(@naam,@lengte,@gewicht)";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@naam",s.Naam);
                    if (s.Lengte==null) cmd.Parameters.AddWithValue("@lengte",DBNull.Value);
                    else cmd.Parameters.AddWithValue("@lengte", s.Lengte);
                    if (s.Gewicht==null) cmd.Parameters.AddWithValue("@gewicht", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@gewicht", s.Gewicht);
                    cmd.CommandText= query;
                    int newID=(int)cmd.ExecuteScalar();
                    s.ZetId(newID);
                    return s;
                }
                catch (Exception ex) { throw new RepositoryException("SchrijfSpeler",ex); }
            }
        }
        public IReadOnlyList<SpelerInfo> SelecteerSpeler(int? id, string naam)
        {
            string sql = "SELECT [Id],t1.[Naam],[Gewicht],[Lengte],[Rugnummer], case when t2.Stamnummer is null then null   else concat(t2.naam,' (',t2.bijnaam,') - ',t2.stamnummer) end teamnaam FROM [Speler] t1 left join team t2 on t1.Team_id=t2.Stamnummer ";
            if (id.HasValue) sql += "WHERE t1.id=@id";
            else sql += "WHERE t1.naam=@naam";
            List<SpelerInfo> spelers= new List<SpelerInfo>();
            using(SqlConnection con = new SqlConnection(connectionString))
            using(SqlCommand cmd=con.CreateCommand())
            {
                if (id.HasValue) cmd.Parameters.AddWithValue("@id", id);
                else cmd.Parameters.AddWithValue("@naam", naam);
                cmd.CommandText = sql;
                con.Open();
                try
                {
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string teamnaam = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("teamnaam"))) teamnaam = (string)reader["teamnaam"];
                        //rest 
                        //SpelerInfo speler = new SpelerInfo();
                        //spelers.Add(speler);
                        
                    }
                    reader.Close();
                    return spelers;
                }
                catch(Exception ex) { throw new RepositoryException("SelecteerSpeler", ex); }
            }

        }
        public Speler SelecteerSpeler(int id)
        {
            string sql = "SELECT t1.id spelerid,t1.naam spelernaam,t1.rugnummer spelerrugnummer,\r\n       t1.lengte spelerlengte,t1.gewicht spelergewicht,t1.teamid spelerteamid,tt.*\r\nFROM speler t1 \r\nleft join\r\n(select t1.Stamnummer,t1.naam ploegnaam,t1.bijnaam,t2.*\r\nfrom team t1 left join speler t2 on t1.Stamnummer=t2.Team_id) tt\r\non t1.Team_id=tt.Stamnummer\r\nwhere t1.id=@spelerid";
            using(SqlConnection con = new SqlConnection(connectionString))
            using(SqlCommand cmd=con.CreateCommand())
            {
                cmd.CommandText=sql;
                cmd.Parameters.AddWithValue("@spelerid", id);
                con.Open();
                Team team = null;
                Speler speler= null;
                try
                {
                    IDataReader reader=cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        if (speler == null)
                        {
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelerlengte"))) lengte = (int)reader["spelerlengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("spelergewicht"))) gewicht= (int)reader["spelergewicht"];
                            speler = new Speler(id, (string)reader["spelernaam"], lengte, gewicht);
                            //rugnr TODO
                            if (reader.IsDBNull(reader.GetOrdinal("spelerteamid"))) return speler;
                        }
                        if (team == null)
                        {
                            string naam = (string)reader["ploegnaam"];
                            team = new Team((int)reader["stamnummer"], naam);

                            //zetbijnaam TODO
                        }
                        //lees alle spelers
                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            int? lengte = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lengte"))) lengte = (int)reader["lengte"];
                            int? gewicht = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("gewicht"))) gewicht = (int)reader["gewicht"];
                            int sid = (int)reader["id"];
                            Speler s = new Speler(id, (string)reader["naam"], lengte, gewicht);
                            s.ZetTeam(team);
                            if (sid == id) speler = s;
                        }
                    }
                    reader.Close();
                    return speler;
                }
                catch(Exception) { throw new RepositoryException("SelecteerSpeler"); }
            }
        }
    }
}
