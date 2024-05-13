using League.BL.Domein;
using League.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ISpelerRepository
    {
        bool BestaatSpeler(Speler s);
        Speler SchrijfSpelerInDB(Speler s);
        IReadOnlyList<SpelerInfo> SelecteerSpeler(int? id, string naam);
    }
}
