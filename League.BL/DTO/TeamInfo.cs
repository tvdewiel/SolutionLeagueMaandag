using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.DTO
{
    public class TeamInfo
    {
        public TeamInfo(int stamnummer, string naam, string bijNaam)
        {
            Stamnummer = stamnummer;
            Naam = naam;
            BijNaam = bijNaam;
        }

        public int Stamnummer {  get; set; }
        public string Naam { get; set; }
        public string BijNaam { get; set; }
    }
}
