using League.BL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Interfaces
{
    public interface ITeamRepository
    {
        bool BestaatTeam(Team team);
        void SchrijfTeamInDB(Team team);
        Team SelecteerTeam(int stamnummer);
    }
}
