using Microsoft.EntityFrameworkCore;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ProjectBase.Repository
{
    /// <summary>
    /// Rename later to EFCoreCRUDModelRepository
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class EfCoreRepository<TModel, TContext>
        : DbContextRepository<TModel, TContext>,
            ICRUDModelRepository<TModel, TContext>
        where TModel : class
        where TContext : DbContext
    {
        protected EfCoreRepository()
            : base() { }

        protected EfCoreRepository(string currentModule)
            : base(currentModule) { }

        public async Task<TModel> GetModelForInsertUpdateFromId(
            object idValue,
            TModel model,
            TContext context
        )
        {
            var argsManual = new NeedDataSourceEventArgs(idValue.ToString(), "Id", Operator.eq);
            var dataInDbResult = await GetKendoGridFiltered(argsManual, context);
            if (dataInDbResult.IsOkay == false)
                return CloneModel(model);
            else
            {
                if (dataInDbResult.Result == null)
                    return CloneModel(model);
                else if (dataInDbResult.Result.Result == null)
                    return CloneModel(model);
                //else if (dataInDbResult.Result.Result.GetType() != typeof(IEnumerable<TModel>))
                //    return CloneModel(model);

                var itemsReturn = (dataInDbResult.Result.Result as IEnumerable<TModel>);
                if (itemsReturn == null || (itemsReturn != null && itemsReturn.Count() > 1))
                    return CloneModel(model);
                else
                    return itemsReturn.FirstOrDefault();
            }
        }

        public virtual Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(NeedDataSourceEventArgs args, TContext context = null)
        {
            //By default we assume this is not a grid
            throw new NotImplementedException();
        }

        public abstract Task<ExecutionResult<TModel>> GetItem(int id, TContext context = null);
        public abstract Task<ExecutionResult<TModel>> Add(TModel model, TContext context = null);
        public abstract Task<ExecutionResult<TModel>> Update(TModel model, TContext context = null);

        public virtual Task<ExecutionResult<TModel>> Delete(TModel model, TContext context = null)
        {
            //By default we do not support delete
            throw new NotImplementedException();
        }

        /// <summary>
        /// Optional to implement..
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<ExecutionResult<TModel>> IsValidGeneral(TModel model, TContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Optional to implement..
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<ExecutionResult<TModel>> IsValidInsert(TModel model, TContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Optional to implement..
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<ExecutionResult<TModel>> IsValidUpdate(TModel model, TContext context)
        {
            throw new NotImplementedException();
        }
    }
}
