//using EFDatabaseModel.Contexts;
//using EFDatabaseModel.DbModel;
//using Microsoft.EntityFrameworkCore;
//using MySql.Data.MySqlClient;
//using ProjectBase.KendoUiSupport;
//using ProjectBase.Repository;
//using ProjectBase.Repository.Transaction;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SchoolManagement.Repositories.SMS
//{
//    public class GuidesReportRepository : DbContextRepository<SchoolMsSmsToSend, EFDatabaseModelDatabaseContext>
//    {

//        public GuidesReportRepository(string currentModule)
//            : base(currentModule)
//        {

//        }

//        public override SchoolMsSmsToSend CloneModel(SchoolMsSmsToSend model)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>> GetKendoGridFiltered(NeedDataSourceEventArgs args, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>(true, "records found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

//                //int filterValueIntId = -1;
//                //string filterValueId = string.Empty;
//                //if (args.HasFilterFields("Id"))
//                //    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);

//                //var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
//                //if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
//                //    filterValueIntId = -1;

//                var accentity = (await context.SchoolMsSmsToSends
//                 .Join(context.SchoolMsLessonGuides, sts => sts.SchoolMsLessonGuideId, g => g.Id, (sts, g) => new { sts, g })
//                  .Join(context.AccountEntities, x => x.sts.EntityId, ae => ae.Id, (x, ae) => new { x, ae })
//                .Where(r => r.x.sts.SentTime!=null)
//                .OrderByDescending(r => r.x.sts.SentTime)
//                .Skip(0)
//                .Take(args.take * args.page)// * args.page
//                //.OrderBy(r => r.ae.Id)
//                .ToListAsync()).Select(r => new SchoolMsSmsToSend
//                {
//                    Id = r.x.sts.Id,
//                   Message = r.x.sts.Message,
//                    DeliveredDate = r.x.sts.DeliveredDate,
//                    SentTime = r.x.sts.SentTime,
//                    PhoneNumber = r.x.sts.PhoneNumber,
//                    Entity=new AccountEntity
//                    {
//                        Id=r.ae.Id,
//                        EntityName=r.ae.EntityName

//                    }

//                });
//                var filterSupporter = new FilterSupporter<SchoolMsSmsToSend>(accentity, args);
//                var filteredData = filterSupporter.FilterData(r => (int)r.Id);
//                executionResult.Result = filterSupporter.itemsResult;
//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }
//    }
//}
