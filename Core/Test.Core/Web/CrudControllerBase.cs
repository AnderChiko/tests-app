using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.Core.Interfaces.Data;
using Test.Core.Interfaces.Web;
using Test.Core.Models.Data;

namespace Test.Core.Web
{
    /// <typeparam name="TController"></typeparam>
    /// <typeparam name="IManager"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class CrudControllerBase<TController, IManager, TModel, TKey> :  ControllerBase, ICrudController<TModel, TKey>
        where IManager : ICrudManager<TModel, TKey>
    {
        protected IManager _manager;
        private string modelName;
        private readonly ILogger<TController> _logger;

        public CrudControllerBase(IManager manager, ILogger<TController> logger) 
        {
            this._manager = manager;
            modelName = typeof(TModel).Name;
            _logger = logger;   
        }

        /// <summary>
        /// Get a result for an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<DataResult<List<TModel>>>> Get()
        {
            try
            {
                var response  = await _manager.Get();

                return response;
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorActionResult(ex, $"{modelName}:GetById", $"{modelName}:Get", "all");
            }
        }

        /// <summary>
        /// Get a result for an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DataResult<TModel>>> Get(TKey id)
        {
            try
            {
                return await _manager.Get(id);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorActionResult(ex, $"{modelName}:GetById", $"{modelName}:GetById", id);
            }
        }
       


        /// <summary>
        /// Perform a post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<DataResult<TModel>>> Post([FromBody] TModel model)
        {
            try
            {
                return await _manager.Create(model);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorActionResult(ex, $"{modelName}:Post", $"{modelName}:Post", model);
            }
        }

        /// <summary>
        /// Perform a put for an id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResult<TModel>>> Put([FromBody] TModel model)
        {
            try
            {
                return await _manager.Update(model);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorActionResult(ex, $"{modelName}:Put", $"{modelName}:Put", model);
            }
        }

        /// <summary>
        /// Perform delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(TKey id)
        {
            try
            {
                return await _manager.Delete(id);
            }
            catch (Exception ex)
            {
                return BuildAndLogErrorActionResult(ex, $"{modelName}:Delete", $"{modelName}:Delete", id);
            }
        }

        protected ActionResult BuildAndLogErrorActionResult(Exception ex, string label, string message, params object[] dataObjects)
        {
            // TODO: Do something to help provide identifying info into error message, perhaps override on ToString of models
            // or an expression pulling out id etc?
            return new BadRequestObjectResult(ex);
        }
    }
}
