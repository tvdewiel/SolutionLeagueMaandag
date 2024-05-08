using League.BL.Domein;
using League.BL.Exceptions;
using League.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Managers
{
    public class TeamManager
    {
        private ITeamRepository repo;

        public TeamManager(ITeamRepository repo)
        {
            this.repo = repo;
        }

        public void RegistreerTeam(int stamnummer,string naam,string bijnaam)
        {
            try
            {
                Team team = new Team(stamnummer, naam);
                if (!string.IsNullOrWhiteSpace(bijnaam)) team.ZetBijnaam(bijnaam);
                if (!repo.BestaatTeam(team))
                {
                    repo.SchrijfTeamInDB(team);
                }
                else
                {
                    throw new ManagerException("team bestaat al");
                }

            }
            catch(ManagerException) { throw; }
            catch(Exception ex) { throw new ManagerException("registreerteam", ex); }
        }
        public Team SelecteerTeam(int stamnummer)
        {
            try
            {
                Team team = repo.SelecteerTeam(stamnummer);
                if (team == null) throw new ManagerException("Team niet gevonden");
                return team;
            }
            catch (ManagerException) { throw; }
            catch(Exception ex) { throw new ManagerException("SelecteerTeam", ex); }
        }
    }
}
