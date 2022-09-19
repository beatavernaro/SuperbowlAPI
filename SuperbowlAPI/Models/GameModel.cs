using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SuperbowlAPI.Models
{
    public class GameModel
    {
        
        [Key]
        public int Id { get; set; }
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

        public GameModel(int id, string date, string sB, string winner, int winnerPoints, string loser, int loserPoints, string mVP, string stadium, string city, string state)
        {
            this.Id = id;
            this.Date = date;
            this.SB = sB;
            this.Winner = winner;
            this.WinnerPoints = winnerPoints;
            this.Loser = loser;
            this.LoserPoints = loserPoints;
            this.MVP = mVP;
            this.Stadium = stadium;
            this.City = city;
            this.State = state;
        }

        public GameModel clone()
        {
            return (GameModel)this.MemberwiseClone();
        }
    }
}
