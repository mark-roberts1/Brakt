using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public interface ITournamentFacilitatorFactory
    {
        ITournamentFacilitator GetTournamentFacilitator(BracketType bracketType);
        bool HasFacilitator(BracketType bracketType);
    }
}
