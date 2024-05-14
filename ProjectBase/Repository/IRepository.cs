using System.Security.Claims;
using System.Security.Principal;

namespace ProjectBase.Repository
{
    public interface IRepository<TModel, TContext>
        where TContext : class
    {
        string CurrentModule { get; }
        string ModuleTransactionPrefix { get; set; }
        TModel DbInstance { get; }
        ClaimsPrincipal CurrentPrincipal { get; }
        void SetCurrentPrincipal(ClaimsPrincipal identity);
        T GetCurrentPrincipalAsType<T>()
            where T : class;
        TModel CloneModel(TModel model);
    }
}
