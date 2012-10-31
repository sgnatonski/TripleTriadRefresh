using Newtonsoft.Json;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Models
{
    public interface IPlayer
    {
        [JsonProperty("connectionId")]
        string ConnectionId { get; set; }
        [JsonProperty("isReady")]
        bool IsReady { get; set; }
        [JsonProperty("userName")]
        string UserName { get; }
        [JsonProperty("hand")]
        Hand Hand { get; set; }
        [JsonIgnore]
        string IpAddress { get; set; }
        [JsonIgnore]
        DbPlayer DbEntity { get; }
        [JsonIgnore]
        int CardsFlip { get; set; }

        void CreatePlayHand();
        void Play(Game game, CardCommand command);
        void UpdateStanding(DbGameResult gameResult);
    }
}
