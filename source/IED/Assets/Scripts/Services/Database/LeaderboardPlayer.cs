using MongoDB.Bson;

namespace Services.Database
{
    public class LeaderboardPlayer
    {
        public ObjectId Id { get; set; }
        public string Nickname { get; private set; }
        public int Score { get; private set; }

        public LeaderboardPlayer(string nickname, int score)
        {
            Nickname = nickname;
            Score = score;
        }
    }
}