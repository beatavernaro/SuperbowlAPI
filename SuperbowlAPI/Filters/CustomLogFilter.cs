using Microsoft.AspNetCore.Mvc.Filters;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Logs;
using SuperbowlAPI.Models;

namespace SuperbowlAPI.Filters
{
    public class CustomLogFilter : IResultFilter,  IActionFilter
    {
        private readonly List<int> _sucessStatusCodes;
        private readonly IBaseRepository<GameModel> _repository;
        private readonly Dictionary<int, GameModel> _contextDict;

        public CustomLogFilter(IBaseRepository<GameModel> repository)
        {
            _repository = repository;
            _contextDict = new Dictionary<int, GameModel>();
            _sucessStatusCodes = new List<int>() {StatusCodes.Status200OK, StatusCodes.Status201Created};
        }

        

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "superbowl", StringComparison.InvariantCultureIgnoreCase))
            {
                int id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.CurrentCultureIgnoreCase)
                    || context.HttpContext.Request.Method.Equals("patch", StringComparison.CurrentCultureIgnoreCase)
                    || context.HttpContext.Request.Method.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var sbGame = _repository.GetByKey(id).Result;
                        if (sbGame != null)
                        {
                            var sbClone = sbGame.clone();
                            _contextDict.Add(id, sbGame);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
           /* if (context.HttpContext.Request.Path.Value.StartsWith("/Superbowl", StringComparison.CurrentCultureIgnoreCase))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    var id = int.Parse(context.HttpContext.Request.Path.ToString());
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.CurrentCultureIgnoreCase)
                    || context.HttpContext.Request.Method.Equals("patch", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var afterUpdate = _repository.GetByKey(id).Result;
                        if (afterUpdate != null)
                        {
                            GameModel beforeUpdate;
                            if(_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog(afterUpdate.Id, "Superbowl", afterUpdate.SB, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                            }
                        }
                    } else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
                    {
                        GameModel beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog(beforeUpdate.Id, "Superbowl", beforeUpdate.SB, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            } */
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
