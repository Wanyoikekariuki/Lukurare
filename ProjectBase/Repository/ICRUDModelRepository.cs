using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System.Threading.Tasks;

namespace ProjectBase.Repository
{
    public interface ICRUDModelRepository<TModel, TContext> : IValidatable<TModel, TContext>
        where TContext : class
    {
        Task<ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>> GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            TContext context = null
        );
        Task<ExecutionResult<TModel>> GetItem(int id, TContext context = null);
        Task<ExecutionResult<TModel>> Add(TModel model, TContext context = null);
        Task<ExecutionResult<TModel>> Update(TModel model, TContext context = null);
        Task<ExecutionResult<TModel>> Delete(TModel model, TContext context = null);
    }
}
