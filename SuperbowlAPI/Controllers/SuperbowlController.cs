using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperbowlAPI.Filters;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Models;
using SuperbowlAPI.Repositories;
using System.Text.Json;

namespace SuperbowlAPI.Controllers
{
    [ApiController]
    
    [Route("[controller]")]
    public class SuperbowlController : ControllerBase
    {

        private readonly IBaseRepository<GameModel> _repository;
        private readonly ILogger<SuperbowlController> _logger;
        public SuperbowlController(IBaseRepository<GameModel> repository, ILogger<SuperbowlController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        private GameModel UpdateGameModel(GameModel newData, GameDto entity)
        {
            //newData.Id = entity.Id;
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
        //[CustomLogFilter]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status201Created)]

        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GameDto entity)
        {
            var gameToInsert = new GameModel(id: 0, entity.Date, entity.SB, entity.Winner, entity.WinnerPoints, entity.Loser, entity.LoserPoints, entity.MVP, entity.Stadium, entity.City, entity.State);

            var dbSuperbowl = await _repository.GetByKey(id);
            if(dbSuperbowl == null)
            {
                var newSuperbowl = await _repository.Insert(gameToInsert);
                return Created(string.Empty, newSuperbowl);
            }
            dbSuperbowl = UpdateGameModel(dbSuperbowl, entity);
            var updated = await _repository.Update(id, dbSuperbowl);

            _logger.Log(LogLevel.Information, $"{DateTime.Now} - ID: {JsonSerializer.Serialize(updated.Id)} - Superbowl: {JsonSerializer.Serialize(updated.SB)} - ");
            //01/01/2021 13:45:00 - Game 1 - Counter Strike - Removido
            //<datetime> - Game <id> - <titulo> - <Remover|Alterar (e descrever a alteração)>
            return Ok(updated);
        }
        #endregion

        #region "POST"
        [HttpPost]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] GameDto entity)
        {
            var gameToInsert = new GameModel(id: 0, entity.Date, entity.SB, entity.Winner, entity.WinnerPoints, entity.Loser, entity.LoserPoints, entity.MVP, entity.Stadium, entity.City, entity.State);
            var inserted = await _repository.Insert(gameToInsert);
            return Created(string.Empty, inserted);
        }
        #endregion

        #region "POST WINNER"
        [HttpPost("winner")]
        public async Task<IActionResult> PostWinner([FromBody] GameDto entity, [FromQuery] int points)
        {
            var result = _repository.Get(1, 10).Result;
            var filtered = result.Where(x => x.WinnerPoints > points);
            return Ok(filtered);
        }
        #endregion

        #region "PATCH"
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] GameMVPDto entity)
        {
            var dbSuperbowl = await _repository.GetByKey(id);
            if(dbSuperbowl == null)
            {
                return NoContent();
            }
            dbSuperbowl.MVP = entity.MVP;
            var updated = await _repository.Update(id, dbSuperbowl);
            return Ok(updated);
        }
        #endregion

        #region "DELETE"
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var dbSuperbowl = await _repository.GetByKey(id);
            if(dbSuperbowl == null)
            {
                return NoContent();
            }
            var deleted = await _repository.Delete(id);
            return Ok(deleted);
        }
        #endregion
    };
}
        

        
    
