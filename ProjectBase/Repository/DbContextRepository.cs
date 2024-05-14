using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectBase.Repository
{
    public abstract class DbContextRepository<TModel, TContext> : IRepository<TModel, TContext>
        where TModel : class
        where TContext : DbContext
    {
        public string CurrentModule { get; }
        public string ModuleTransactionPrefix { get; set; }
        public TModel DbInstance { get; protected set; }
        public abstract TModel CloneModel(TModel model);
        public ClaimsPrincipal CurrentPrincipal { get; private set; }

        public virtual void SetCurrentPrincipal(ClaimsPrincipal identity)
        {
            CurrentPrincipal = identity;
        }

        public virtual T GetCurrentPrincipalAsType<T>()
            where T : class
        {
            if (CurrentPrincipal == null)
                return null;
            else
                return CurrentPrincipal as T;
        }

        protected DbContextRepository() { }

        protected DbContextRepository(string currentModule)
        {
            CurrentModule = currentModule;
        }

        protected DbContextRepository(string currentModule, ClaimsPrincipal identity)
        {
            CurrentModule = currentModule;
            SetCurrentPrincipal(identity);
        }
    }
}
