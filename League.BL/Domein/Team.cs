using League.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Domein
{
    public class Team
    {
        public int Stamnummer {  get;private set; }
        public string Naam { get; private set; }
        public string Bijnaam { get; private set; }
        private List<Speler> _spelers = new List<Speler>();

        public Team(int stamnummer, string naam)
        {
            ZetStamnummer(stamnummer);
            ZetNaam(naam);
        }
        public IReadOnlyList<Speler> Spelers() {  return _spelers.AsReadOnly(); }
        public void ZetStamnummer(int id)
        {
            if (id <= 0) throw new SpelerException("ZetStamnummer");
            Stamnummer = id;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new SpelerException("ZetNaam");
            Naam = naam.Trim();
        }
        public void ZetBijnaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new SpelerException("ZetBijnaam");
            Bijnaam = naam.Trim();
        }
        internal void VerwijderSpeler(Speler speler)
        {
            if (speler == null || !_spelers.Contains(speler))  throw new TeamException("VerwijderSpeler");
            _spelers.Remove(speler);
            if (speler.Team==this)
                speler.VerwijderTeam();
        }
        internal void VoegSpelerToe(Speler speler)
        {
            if (speler == null || _spelers.Contains(speler)) throw new TeamException("VoegSpelerToe");
            _spelers.Add(speler);
            if (speler.Team != this)
                speler.ZetTeam(this);
        }

        internal bool HeeftSpeler(Speler speler)
        {
            return _spelers.Contains(speler);
        }
    }
}
