using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SuperbowlAPI.Models
{
    public class GameMVPDto
    {
        public string MVP { get; set; }
        public GameMVPDto(string mvp)
        {
            MVP = mvp;
        }
    }
}
