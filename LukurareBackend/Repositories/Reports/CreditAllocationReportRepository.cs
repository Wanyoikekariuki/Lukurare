using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Repositories.SMS
{
    public class CreditAllocationReportRepository
        : DbContextRepository<BillReferenceServiceRequest, EFDatabaseModelDatabaseContext>
    {
        public CreditAllocationReportRepository(string currentModule)
            : base(currentModule) { }

        public override BillReferenceServiceRequest CloneModel(BillReferenceServiceRequest model)
        {
            throw new NotImplementedException();
        }

        public virtual UserPrinciple GetCurrentPrincipalGenericPrinciple()
        {
            var princip = CurrentPrincipal as UserPrinciple;

            return princip;
        }

        public async Task<ExecutionResult<IEnumerable<AccountEntity>>> GetReport(
           
            NeedDataSourceEventArgs args,
            
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                 IEnumerable<AccountEntity>>(true, "records found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //args = NeedDataSourceEventArgs.EnsureArgsValid(args);


                var entities = await context.AccountEntities
                    .Join(context.AccountEntityTypeAdditionalDetailsValues, ae => ae.Id, adv => adv.AccountEntityId, (ae, adv) => new { ae, adv })
                    .Join(context.AccountEntityTypeAdditionalDetails, b => b.adv.TypeAdditionalDetailsId, adt => adt.Id, (b, adt) => new {b, adt})
                    .ToListAsync();


                var entityGroup = entities.GroupBy(m => new { Id = m.b.ae.Id, KeyName = m.adt.KeyName })
                    .Select(g => new
                    {
                        AccountEntityId = g.Key.Id,
                        KeyName = g.Key.KeyName,
                        Name = g.FirstOrDefault().b.ae.EntityName,
                        UserName = g.FirstOrDefault().b.ae.EntityUserName,
                        Values = g.Select(d => d.b.adv.Value).ToList()
                    });

                var entityGroupAll = entityGroup.GroupBy(k => new
                {
                    k.AccountEntityId          
                })
                .Select(g => new
                {
                 AccountEntityId = g.Key.AccountEntityId,
                 Age = g.FirstOrDefault(e => e.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.Age).Values.FirstOrDefault(),
                 IdNo = g.FirstOrDefault(e => e.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.NationaIDNumber).Values.FirstOrDefault(),
                 Education = g.FirstOrDefault(e => e.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.HighestLevelOfEducation).Values.FirstOrDefault(),
                 EntityName = g.FirstOrDefault().Name,
                 EntityUserName = g.FirstOrDefault().UserName
                })
                .ToList();


                IEnumerable<AccountEntity> convertedAccentity = entityGroupAll.Select(m =>
                   new AccountEntity
                   {
                       Id = m.AccountEntityId,
                       EntityNo = m.Age, //EntityNo used as workaround to store total.
                       EntityName = m.EntityName,
                       EntityUserName = m.EntityUserName,
                       Phone2 = m.IdNo, // Phone2 used as workaround to store AccountNumber
                       PostalAddress = m.Education
                   });

                executionResult.Result = convertedAccentity;
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }
    }
}
