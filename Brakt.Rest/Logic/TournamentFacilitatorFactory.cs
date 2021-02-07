using Brakt.Rest.Data;

namespace Brakt.Rest.Logic
{
    public class TournamentFacilitatorFactory : ITournamentFacilitatorFactory
    {
        private readonly IDataLayer _dataLayer;
        private readonly IStatsGenerator _statsGenerator;

        public TournamentFacilitatorFactory(IDataLayer dataLayer, IStatsGenerator statsGenerator)
        {
            _dataLayer = dataLayer;
            _statsGenerator = statsGenerator;
        }

        public ITournamentFacilitator GetTournamentFacilitator(BracketType bracketType)
        {
            return bracketType switch
            {
                BracketType.Swiss => new SwissTournamentFacilitator(_dataLayer, _statsGenerator),
                BracketType.SingleElimination => new SingleElimTournamentFacilitator(_dataLayer, _statsGenerator),
                BracketType.RoundRobin => new RoundRobinTournamentFacilitator(_dataLayer, _statsGenerator),
                _ => null,
            };
        }

        public bool HasFacilitator(BracketType bracketType)
        {
            return GetTournamentFacilitator(bracketType) != null;
        }
    }
}
