using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Database
{
    public interface IDatabaseService : IService
    {
        IEnumerable<LeaderboardPlayer> Leaderboard { get; }

        void AddToLeaderboard(string nickname, int score);

        Task<IEnumerable<LeaderboardPlayer>> UpdateTopPlayers();

        bool IsNeedToUpdateLeaderboard();
    }
}