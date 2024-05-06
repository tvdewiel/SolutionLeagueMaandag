using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Domein
{
    public class Transfer
    {
        public Transfer(Speler speler, Team oudTeam)
        {
            ZetSpeler(speler);
            ZetOudTeam(oudTeam);
            ZetPrijs(0);
        }

        public Transfer(Speler speler, Team nieuwTeam, int prijs) 
        {
            ZetPrijs(prijs);
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
        }

        public Transfer(Speler speler, Team nieuwTeam, Team oudTeam, int prijs)
        {
            ZetPrijs(prijs);
            ZetSpeler(speler);
            ZetNieuwTeam(nieuwTeam);
            ZetOudTeam(oudTeam);
        }

        public int? Id { get; private set; }
        public Speler Speler { get; private set; }
        public Team NieuwTeam { get; private set; }
        public Team OudTeam { get; private set; }
        public int Prijs {  get; private set; }
        public void ZetId(int id)
        {
            if (id <= 0) throw new TransferException("ZetId");
            Id = id;
        }
        public void ZetPrijs(int prijs)
        {
            if (prijs < 0) throw new TransferException("ZetPrijs");
            Prijs = prijs;
        }
        public void ZetSpeler(Speler speler)
        {
            if (speler == null) throw new TransferException("ZetSpeler");
            Speler = speler;
        }
        public void ZetNieuwTeam(Team team)
        {
            if (team == null) throw new TransferException("ZetNieuwTeam");
            if (team==OudTeam) throw new TransferException("ZetNieuwTeam");
            NieuwTeam = team;
        }
        public void ZetOudTeam(Team team)
        {
            if (team == null) throw new TransferException("ZetOudTeam");
            if (team == NieuwTeam) throw new TransferException("ZetOudTeam");
            OudTeam = team;
        }
        public void VerwijderOudTeam()
        {
            if (NieuwTeam is null) throw new TransferException("VerwijderOudTeam");
            OudTeam = null;
        }
        public void VerwijderNieuwTeam()
        {
            if (OudTeam is null) throw new TransferException("VerwijderNieuwTeam");
            NieuwTeam = null;
        }
    }
}
