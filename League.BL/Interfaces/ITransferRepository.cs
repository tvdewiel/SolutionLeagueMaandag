using League.BL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ITransferRepository
    {
        Transfer SchrijfTransferInDB(Transfer transfer);
    }
}
