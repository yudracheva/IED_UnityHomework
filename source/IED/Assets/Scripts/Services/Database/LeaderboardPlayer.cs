using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Services.Database
{
    [DataContract]
    [BsonIgnoreExtraElements]
    public class LeaderboardPlayer
    {
        public LeaderboardPlayer() { }

        public LeaderboardPlayer(string nickname, int score)
        {
            Nickname = nickname;
            Score = score;
        }

        [DataMember]
        [BsonId]
        public ObjectId Id { get; set; }
        
        [DataMember]
        [BsonElement("Nickname")]
        public string Nickname { get; set;}
        
        [DataMember]
        [BsonElement("Score")]
        public int Score { get; set;}
    }
}