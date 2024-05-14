using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectBase.Database.Connection;

namespace EFDatabaseModel.Contexts
{
    public class EFDatabaseModelDatabaseContext : orgosContext
    {
        public EFDatabaseModelDatabaseContext() { }

        //public readonly ILoggerFactory _loggerFactory;
        public EFDatabaseModelDatabaseContext(DbContextOptions<orgosContext> options /*ILoggerFactory loggerFactory*/)
            : base(options) 
        {
            //_loggerFactory = loggerFactory;
        }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ContextConnectionService.GetConnectionString(
                ContextConnectionService.DefaultConnectionStringName
            );
            //optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
