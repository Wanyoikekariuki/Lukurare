using EFDatabaseModel.Contexts;
using EFDatabaseModel.Models.Authentication;
using ProjectBase.Repository;

namespace EFDatabaseModel.Repository
{
    //  EF Generate DB Scaffold-DbContext -Connection name=bulkSMSGatewayConnectionString -Provider MySql.EntityFrameworkCore -Project "EFDatabaseModel" -OutputDir "DbModel" -Force
    /// <summary>
    /// Base repo for bulk sms Context Based Classes, Rename to CRUD
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class EFDatabaseModelBaseRepository<TModel>
        : EfCoreRepository<TModel, EFDatabaseModelDatabaseContext>
        where TModel : class
    {
        protected EFDatabaseModelBaseRepository(string currentModule)
            : base(currentModule) { }

        public UserPrinciple UserPrinciple
        {
            get { return base.GetCurrentPrincipalAsType<UserPrinciple>(); }
        }
    }
}
