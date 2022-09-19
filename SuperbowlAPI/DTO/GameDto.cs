using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SuperbowlAPI.Models
{
    public class GameDto
    {
        public string Date { get; set; }
        public string SB { get; set; }
        public string Winner { get; set; }

        [JsonPropertyName("Winner Pts")]
        public int WinnerPoints { get; set; }
        public string Loser { get; set; }

        [JsonPropertyName("Loser Pts")]
        public int LoserPoints { get; set; }
        public string MVP { get; set; }
        public string Stadium { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}
