using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.SMS
{
    public class SMSOutBoxRepository
        : DbContextRepository<SchoolMsSmsToSend, EFDatabaseModelDatabaseContext>
    {
        public SMSOutBoxRepository(string currentModule)
            : base(currentModule) { }

        public override SchoolMsSmsToSend CloneModel(SchoolMsSmsToSend model)
        {
            throw new NotImplementedException();
        }

        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "AccountEntityAppliedSender instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed

                #region filter sections

                string phone_number = null;
                if (args.HasFilterFields("FromPhone"))
                    phone_number = args.GetFilterValuePredicate(r => r.field == "FromPhone", 0);

                //DateTime datesent = default(DateTime);
                //string datesentStringValue = null;
                //if (args.HasFilterFields("Datesent"))
                //    datesentStringValue = args.GetFilterValuePredicate(r => r.field == "Datesent", 0);
                //if (!string.IsNullOrEmpty(datesentStringValue))
                //    DateTime.TryParse(datesentStringValue, out datesent);


                #endregion

                string whereClause =
                    @" 1 and sent_time is not null and school_ms_lesson_id IS NULL
                        AND school_ms_lesson_guide_id IS NULL
                        AND school_ms_student_assessment_question_sms_schedule_id IS NULL
                        AND batch_job_id IS NULL
                        AND message not like '%password(OTP)%'";

                var listParams = new List<DbParameter>();
                if (!string.IsNullOrEmpty(phone_number))
                {
                    listParams.Add(new MySqlParameter("phone_number", phone_number));
                    whereClause = $" and {whereClause} and phone_number = @phone_number ";
                }

                var sqlQuery =
                    $@"
                SELECT 
                    *
                FROM
                    school_ms_sms_to_send
                WHERE
                     {whereClause} 
                order by id desc 
                limit @skip,@take";

                listParams.Add(new MySqlParameter("skip", 0));
                listParams.Add(new MySqlParameter("take", args.take * args.page));

                var smsInboxData = await context.SchoolMsSmsToSends
                    .FromSqlRaw(sqlQuery, listParams.ToArray())
                    .ToListAsync();

                var filterSupporter = new FilterSupporter<SchoolMsSmsToSend>(smsInboxData, args);
                var filteredData = filterSupporter.FilterData(r => (int)r.Id);
                executionResult.Result = filterSupporter.itemsResult;
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

        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetSMSMessageQueueData(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "AccountEntityAppliedSender instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed

                #region filter sections

                string phone_number = null;
                if (args.HasFilterFields("FromPhone"))
                    phone_number = args.GetFilterValuePredicate(r => r.field == "FromPhone", 0);

                #endregion

                string whereClause = " 1 and sent_time is null ";
                var listParams = new List<DbParameter>();
                if (!string.IsNullOrEmpty(phone_number))
                {
                    listParams.Add(new MySqlParameter("phone_number", phone_number));
                    whereClause = $" and {whereClause} phone_number = @phone_number ";
                }

                var sqlQuery =
                    $@"
                SELECT 
                    *
                FROM
                    school_ms_sms_to_send
                WHERE
                     {whereClause} 
                order by id desc 
                limit @skip,@take";

                listParams.Add(new MySqlParameter("skip", 0));
                listParams.Add(new MySqlParameter("take", args.take * args.page));

                var smsInboxData = await context.SchoolMsSmsToSends
                    .FromSqlRaw(sqlQuery, listParams.ToArray())
                    .ToListAsync();

                var filterSupporter = new FilterSupporter<SchoolMsSmsToSend>(smsInboxData, args);
                var filteredData = filterSupporter.FilterData(r => (int)r.Id);
                executionResult.Result = filterSupporter.itemsResult;
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
