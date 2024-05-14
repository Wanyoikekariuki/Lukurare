using CommunicationLibrary.Gateways.Config;
using CommunicationLibrary.Gateways.Telephony.SMS;
using CommunicationLibrary.Telephony.Messaging.SMS;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository;
using ProjectBase.Repository.Transaction;
using LukurareBackend.Models.SMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.SMS
{
    public class SMSMessageSenderRepository
        : DbContextRepository<SendSMSModel, EFDatabaseModelDatabaseContext>
    {
        public SMSGatewayDefination SMSGatewayDefination { get; set; }

        public SMSMessageSenderRepository(string currentModule)
            : base(currentModule) { }

        public SMSMessageSenderRepository(
            string currentModule,
            SMSGatewayDefination smsGatewayDefination
        )
            : this(currentModule)
        {
            this.SMSGatewayDefination = smsGatewayDefination;
        }

        public bool UsesGroupContacts { get; private set; }

        public override SendSMSModel CloneModel(SendSMSModel model)
        {
            throw new NotImplementedException();
        }

        //upload the messages on diffrent mode

        public async Task<ExecutionResult<SendSMSModel>> SendSMSFromModel(
            SendSMSModel model,
            string projectRootFolder,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SendSMSModel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                var listPhoneNumbers = new List<string>();
                if (!string.IsNullOrEmpty(model.PhoneNumbers))
                    listPhoneNumbers = model.PhoneNumbers.Split(',').ToList();
                if (UsesGroupContacts)
                {
                    var groupContacts = await (
                        context.AccountEntityGroups
                            .Join(
                                context.AccountEntityGroupContacts,
                                ae => ae.Id,
                                gc => gc.AccountEntityGroupId,
                                (ae, gc) => new { ae, gc }
                            )
                            .Join(
                                context.AccountEntityContacts,
                                aegc => aegc.gc.AccountEntityContactId,
                                aec => aec.Id,
                                (aegc, aec) => new { aegc, aec }
                            )
                            .Where(r => r.aegc.ae.Id == model.AccountEntityGroup.Id)
                    )
                        .Select(r => new { PhoneNumber = r.aec.ContactPhone })
                        .ToListAsync();

                    listPhoneNumbers.AddRange(groupContacts.Select(r => r.PhoneNumber));
                }

                var excelPhoneNumbersRes = getPhoneNumberFromFiles(projectRootFolder, model);
                if (excelPhoneNumbersRes.IsOkay == false)
                    return excelPhoneNumbersRes.CloneDiffrentType<SendSMSModel>();
                listPhoneNumbers.AddRange(excelPhoneNumbersRes.Result);

                if (listPhoneNumbers.Any() == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "No contacs could be loaded for this request";
                    return executionResult;
                }

                //remove any duplicate phone numbers
                listPhoneNumbers = listPhoneNumbers
                    .Select(r => r.ToLower())
                    .GroupBy(r => r)
                    .Select(r => r.FirstOrDefault())
                    .ToList();
                var communicationRepo = new SMSTextMessageDefaultGateway(
                    CurrentModule,
                    SMSGatewayDefination
                );
                var smsMessage = new SMSTextMessage(
                    model.MessageToSend,
                    SMSGatewayDefination.DefaultMaskName,
                    listPhoneNumbers
                );
                smsMessage.MessageScheduleDate = model.ScheduledDate;

                var resultSend = communicationRepo.SendSMS(
                    new HashSet<SMSTextMessage> { smsMessage }
                );
                if (resultSend.IsOkay == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Queueing messages failed. Error {resultSend.Message}";
                    executionResult.TechnicalMessage = resultSend.TechnicalMessage;
                    return executionResult;
                }

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Message was submitted ";
                    executionResult.Result = model;
                }
                else
                {
                    //nothing in DB To return
                }
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

        public ExecutionResult<IEnumerable<string>> UploadSMSFile(
            string webRootPath,
            List<string> fileNames
        )
        {
            fileNames = fileNames
                .Select(r =>
                {
                    return r.Replace($"{webRootPath}", "").Replace("\\", "/");
                })
                .ToList();
            return new ExecutionResult<IEnumerable<string>>(true, "", fileNames);
        }

        public static string SMSExcelFolderName = "UploadedFiles\\SMS";

        public static string RootFolderSMSName(string projectRootFolder)
        {
            return $"{projectRootFolder}\\{SMSExcelFolderName}";
        }

        public static string RootFolderFileSMSName(string projectRootFolder, string fileName)
        {
            return $"{RootFolderSMSName(projectRootFolder)}\\{fileName}";
        }

        private ExecutionResult<IEnumerable<string>> getPhoneNumberFromFiles(
            string projectRootFolder,
            SendSMSModel model
        )
        {
            //https://stackoverflow.com/questions/33436525/how-to-parse-excel-rows-back-to-types-using-epplus//
            //https://long2know.com/2016/05/parsing-excel-to-a-list-of-objects/
            //use this link for mapping
            var result = new ExecutionResult<IEnumerable<string>>(true, "", new HashSet<string>());
            var listPhoneNumbers = new List<string>();
            foreach (var file in model.UploadedFileLinks)
            {
                var correctedLink = file.Replace("/", "\\");
                var existingFile = new FileInfo($"{projectRootFolder}\\{correctedLink}");
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                if (existingFile.Exists == false)
                {
                    result.IsOkay = false;
                    result.Message = $"File {correctedLink} does not exist";
                    return result;
                }
                using (var package = new ExcelPackage(existingFile))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    if (package.Workbook.Worksheets.Any() == false)
                    {
                        result.IsOkay = false;
                        result.Message = $"{file} does not have any work sheets";
                        return result;
                    }
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        var noOfCol = sheet.Dimension.End.Column;
                        var noOfRow = sheet.Dimension.End.Row;
                        if (noOfCol < 0)
                        {
                            result.IsOkay = false;
                            result.Message =
                                $"sheet {sheet.Index} in file {file} does not have any columns. Alteast one column is required";
                            return result;
                        }
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var phoneNumber = sheet.Cells[rowIterator, 1].Value.ToString();
                            listPhoneNumbers.Add(phoneNumber);
                        }
                    }
                }
            }
            result.Result = listPhoneNumbers;
            return result;
        }

        public async Task<ExecutionResult<IEnumerable<AccountEntityGroup>>> GetTargetMessageGroups(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<IEnumerable<AccountEntityGroup>>(
                true,
                "AccountEntityAppliedSender instance found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed

                var filterValueTop = args.GetFilterValue(0);

                var groupContacts = (
                    await context.AccountEntityGroups
                        .Where(
                            r =>
                                string.IsNullOrEmpty(filterValueTop)
                                    ? true
                                    : r.GroupName.Contains(filterValueTop)
                        )
                        .ToListAsync()
                ).Select(r => new AccountEntityGroup { Id = r.Id, GroupName = r.GroupName, });

                executionResult.Result = groupContacts;
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

        private async Task<ExecutionResult<SendSMSModel>> IsValidInsert(
            SendSMSModel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = new ExecutionResult<SendSMSModel>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is empty";
                return result;
            }
            if (string.IsNullOrEmpty(model.MessageToSend))
            {
                result.IsOkay = false;
                result.Message = "Message to send is empty";
                return result;
            }
            if (model.AccountEntityGroup.Id > 0)
            {
                var groupInDb = await context.AccountEntityGroups.FirstOrDefaultAsync(
                    r => r.Id == model.AccountEntityGroup.Id
                );
                if (groupInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "The selected group does not exist";
                    return result;
                }
                UsesGroupContacts = true;
            }

            return result;
        }
    }
}
