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
    public class TransferRepository : ITransferRepository
    {
        private string connectionString;

        public TransferRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Transfer SchrijfTransferInDB(Transfer transfer)
        {
            string sqlTransfer = "INSERT INTO transfer(spelerid,prijs,oudteamid,nieuwteamid)" +
                " output INSERTED.ID VALUES(@spelerid,@prijs,@oudteamid,@nieuwteamid)";
            string sqlSpler = "UPDATE speler SET teamid=@teamid WHERE id=@id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmdTransfer = conn.CreateCommand())
            using (SqlCommand cmdSpeler = conn.CreateCommand())
            {
                conn.Open();
                SqlTransaction transactie = conn.BeginTransaction();
                try
                {
                    cmdSpeler.Transaction = transactie;
                    cmdTransfer.Transaction = transactie;
                    //transfer
                    cmdTransfer.CommandText = sqlTransfer;
                    cmdTransfer.Parameters.AddWithValue("@spelerid", transfer.Speler.Id);
                    cmdTransfer.Parameters.AddWithValue("@prijs", transfer.Prijs);
                    if (transfer.OudTeam != null)                    cmdTransfer.Parameters.AddWithValue("@oudteamid", transfer.OudTeam.Stamnummer);
                    else cmdTransfer.Parameters.AddWithValue("@oudteamid",DBNull.Value);
                    if (transfer.NieuwTeam != null) cmdTransfer.Parameters.AddWithValue("@nieuwteamid", transfer.NieuwTeam.Stamnummer);
                    else cmdTransfer.Parameters.AddWithValue("@nieuwteamid", DBNull.Value);
                    int newID=(int)cmdTransfer.ExecuteScalar();
                    transfer.ZetId(newID);
                    //speler
                    cmdSpeler.CommandText = sqlSpler;
                    cmdSpeler.Parameters.AddWithValue("@id",transfer.Speler.Id);
                    if (transfer.NieuwTeam == null) cmdSpeler.Parameters.AddWithValue("@teamid", DBNull.Value);
                    else cmdSpeler.Parameters.AddWithValue("@teamid", transfer.NieuwTeam.Stamnummer);
                    cmdSpeler.ExecuteNonQuery();
                    transactie.Commit();
                    return transfer;
                }
                catch (Exception ex)
                {
                    transactie.Rollback();
                    throw new RepositoryException("Schrijftransfer");
                }
            }
        }
    }
}
