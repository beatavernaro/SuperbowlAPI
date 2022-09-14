using Microsoft.AspNetCore.Mvc;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Models;
using SuperbowlAPI.Repositories;

namespace SuperbowlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuperbowlController : ControllerBase
    {

        private readonly IBaseRepository<GameModel> _repository;
        public SuperbowlController(IBaseRepository<GameModel> repository)
        {
            _repository = repository;
        }
        /*
         * public int Id { get; set; }
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
         */
        private GameModel UpdateGameModel(GameModel newData, GameModel entity)
        {
            newData.Id = entity.Id;
            newData.Date = entity.Date;
            newData.SB = entity.SB;
            newData.Winner = entity.Winner;
            newData.WinnerPoints = entity.WinnerPoints;
            newData.Loser = entity.Loser;
            newData.LoserPoints = entity.LoserPoints;
            newData.MVP = entity.MVP;
            newData.Stadium = entity.Stadium;
            newData.City = entity.City;
            newData.State = entity.State;
            return newData;
        }

        #region "GET"
        [HttpGet]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResults)
        {
            var superbowl = await _repository.Get(page, maxResults);
            if(superbowl.ToList() == null)
            {
                return NotFound("Necessário número da pagina e quantidade de valores");
            }
            return Ok(superbowl);
        }
        #endregion

        #region "GET BY ID"
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var superbowl = await _repository.GetByKey(id);
            if(superbowl == null)
            {
                return NotFound("ID não existe");
            }
            return Ok(superbowl);
        }
        #endregion

        #region "PUT"
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status201Created)]

        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GameModel entity)
        {
            var dbSuperbowl = await _repository.GetByKey(id);
            if(dbSuperbowl == null)
            {
                var newSuperbowl = await _repository.Insert(entity);
                return Created(string.Empty, newSuperbowl);
            }
            dbSuperbowl = UpdateGameModel(dbSuperbowl, entity);
            var updated = await _repository.Update(id, dbSuperbowl);
            return Ok(updated);
        }
        #endregion

    };
}
        

        
    
