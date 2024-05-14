//using LukurareBackend.Repositories.LoadAirtime;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.Repository;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.KendoUiSupport;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Controllers.Repository
{
    [Route("api/[controller]")]
    public class BulkSMSGatewayBaseGridController<TModel, TRepository>
        : ContextModelBaseController<TModel, TRepository, EFDatabaseModelDatabaseContext>
        where TModel : class, new()
        where TRepository : EFDatabaseModelBaseRepository<TModel>
    {
        //private LoadMeterRepository loadMeterRepository;

        //private AirtimeManagement.Repositories.LoadAirtime.MeterOperationRepository meterOperationRepository;

        //public AirtimeManagement.Repositories.LoadAirtime.MeterOperationRepository MeterOperationRepository { get; }

        //we use pairs of fromBody and Empty for this link reason
        //https://andrewlock.net/model-binding-json-posts-in-asp-net-core/

        public BulkSMSGatewayBaseGridController(TRepository repository)
            : base(repository) { }

        #region get
        [Route("GetKendoGridFiltered"), HttpPost]
        public virtual async Task<IActionResult> GetKendoGridFiltered(
            [FromBody] NeedDataSourceEventArgs args
        )
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetKendoGridFiltered(args);
            return GetActionResult(result);
        }

        [Route("GetKendoGridFilteredFromQuery"), HttpPost]
        public virtual async Task<IActionResult> GetKendoGridFilteredFromQuery(
            NeedDataSourceEventArgs args
        )
        {
            Repository.SetCurrentPrincipal(User);
            return await GetKendoGridFiltered(args);
        }

        [Route("GetItem/{id}"), HttpGet]
        public virtual async Task<IActionResult> GetItem(int id)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.GetItem(id);
            return GetActionResult(result);
        }
        #endregion

        #region update section
        [Route("Update"), HttpPut]
        public virtual async Task<IActionResult> Update([FromBody] TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Update(model);
            return GetActionResult(result);
        }

        [Route("UpdateFromQuery"), HttpPut]
        public virtual async Task<IActionResult> UpdateFromQuery(TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            return await Update(model);
        }
        #endregion

        #region add
        [Route("Add"), HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Add(model);
            return GetActionResult(result);
        }

        [Route("AddFromQuery"), HttpPost]
        public virtual async Task<IActionResult> AddFromQuery(TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            return await Add(model);
        }
        #endregion

        #region delete
        [Route("Delete"), HttpPost]
        public virtual async Task<IActionResult> Delete([FromBody] TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            var result = await Repository.Delete(model);
            return GetActionResult(result);
        }

        [Route("DeleteFromQuery"), HttpPost]
        public virtual async Task<IActionResult> DeleteFromQuery(TModel model)
        {
            Repository.SetCurrentPrincipal(User);
            return await Delete(model);
        }
        #endregion
    }
}
