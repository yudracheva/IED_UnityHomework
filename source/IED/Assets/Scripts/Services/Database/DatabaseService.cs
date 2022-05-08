using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstantsValue;
using MongoDB.Driver;
using UnityEngine;

namespace Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<LeaderboardPlayer> _databaseCollection;
        private float _lastUpdateTime;

        public DatabaseService()
        {
            var client = new MongoClient(Constants.MongoClientSettings);
            var database = client.GetDatabase(Constants.MongoDatabaseName);
            _databaseCollection = database.GetCollection<LeaderboardPlayer>(Constants.MongoCollectionName);
            _lastUpdateTime = -Constants.LeaderboardUpdateRange;
        }

        public IEnumerable<LeaderboardPlayer> Leaderboard { get; private set; }

        public async void AddToLeaderboard(string nickname, int score)
        {
            await _databaseCollection.InsertOneAsync(DatabaseElement(nickname, score));
        }

        public async Task<IEnumerable<LeaderboardPlayer>> UpdateTopPlayers()
        {
            var collection = await _databaseCollection.FindAsync(FilterDefinition<LeaderboardPlayer>.Empty);
            var listCollection = await collection.ToListAsync();
            var leaderboardPlayers = listCollection.OrderByDescending(x => x.Score).Take(Constants.TopLength);
            Leaderboard = leaderboardPlayers;
            UpdateLastUpdateTime();
            return Leaderboard;
        }

        public bool IsNeedToUpdateLeaderboard()
        {
            return Time.time >= _lastUpdateTime + Constants.LeaderboardUpdateRange;
        }

        private static LeaderboardPlayer DatabaseElement(string nickname, int score)
        {
            return new(nickname, score);
        }

        private void UpdateLastUpdateTime()
        {
            _lastUpdateTime = Time.time;
        }
    }
}